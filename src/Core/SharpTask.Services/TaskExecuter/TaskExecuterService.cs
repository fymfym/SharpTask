using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Services.TaskDirectoryFileService;
using SharpTask.Core.Services.TaskDirectoryManipulation;
using SharpTask.Core.Services.TaskDllLoader;
using SharpTask.Core.Services.TaskExecution;

namespace SharpTask.Core.Services.TaskExecuter
{
    public class TaskExecuterService : ITaskExecuterService
    {
        private readonly ILogger<TaskExecuterService> _logger;
        private readonly IAssemblyCollectionService _assemblyCollectionService;
        private readonly ITaskDllLoaderService _taskDllLoaderService;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;
        private readonly ITaskExecutionService _taskExecutionService;

        bool _running;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<long, TaskInformation> _activeAssemblies;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<long, TaskInformation> _closedAssemblies;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<long, TaskClassState> _activeTaskClasses;

        public TaskExecuterService(
            ILogger<TaskExecuterService> logger,
            IAssemblyCollectionService assemblyCollectionService,
            ITaskDllLoaderService taskDllLoaderService,
            ITaskDirectoryManipulationService taskDirectoryManipulationService,
            ITaskExecutionService taskExecutionService
            )
        {
            _logger = logger;
            _assemblyCollectionService = assemblyCollectionService;
            _taskDllLoaderService = taskDllLoaderService;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;
            _taskExecutionService = taskExecutionService;

            _activeAssemblies = new Dictionary<long, TaskInformation>();
            _activeTaskClasses = new Dictionary<long, TaskClassState>();
            _closedAssemblies = new Dictionary<long, TaskInformation>();
        }

        public void Start()
        {
            _logger.LogInformation("Starting SharpTaskExecuter");
            _running = true;

            while (_running)
            {

                var unloadableAssembliesTask = _assemblyCollectionService.GetUnloadableTaskDirectories();
                var newAssembliesTask = _assemblyCollectionService.GetNewTaskDirectories();
                Task.WaitAll(unloadableAssembliesTask, newAssembliesTask);
                HandleUnloadableAssemblies(unloadableAssembliesTask.Result);
                HandleNewAssemblies(newAssembliesTask.Result);

                var runnableAssembliesTask = _assemblyCollectionService.GetRunnableTaskDirectories();
                Task.WaitAll(runnableAssembliesTask);
                LoadRunnableAssemblies(runnableAssembliesTask.Result);

                ExecuteClasses();
            }
        }

        public void Stop()
        {
            _running = false;
        }


        private void LoadRunnableAssemblies(IEnumerable<TaskInformation> taskInformation)
        {
            foreach (var assemblyInformation in taskInformation)
            {

                var classes = from assembly in assemblyInformation.Domain.GetAssemblies()
                              where assembly.ExportedTypes.Any()
                              from exported in assembly.ExportedTypes
                              where exported.IsClass
                              select exported as TypeInfo;

                foreach (var cls in classes)
                {
                    if (cls.ImplementedInterfaces.Any(x =>
                        x.FullName.Equals("SharpTask.Core.Models.Task.ISharpTask")))
                    {
                        var instance = (ISharpTask)Activator.CreateInstance(cls);
                        _logger.LogInformation("{@action}{@taskName}{@Tasktype}{@TaskDescription}",
                            "Instaciation class",
                            instance.Name,
                            instance.GetType(),
                            instance.Description);


                        var sharpClass = new TaskClassState(assemblyInformation, instance);

                        _activeTaskClasses.Add(sharpClass.GetHashCode(), sharpClass);
                    }
                }

            }
        }

        private void HandleNewAssemblies(IEnumerable<TaskInformation> taskInformation)
        {
            foreach (var task in taskInformation)
            {
                if (_activeAssemblies.ContainsKey(task.Hash)) continue;
                _logger.LogInformation("{@action}{@task}",
                    "Adding tasks from pickup folder",
                    task.Domain.BaseDirectory);

                try
                {
                    _logger.LogInformation("{@action}{@assembly}",
                        "Load assembly",
                        task.Domain.BaseDirectory);

                    var loadedAssembly = _taskDllLoaderService.LoadTaskIntoAppDomain(task);

                    _logger.LogInformation("{@action}{@assembly}{@class}",
                        "Loadeed assembly",
                        task.Domain.BaseDirectory,
                        loadedAssembly.GetType());

                    _activeAssemblies.Add(
                        task.Hash,
                        task
                        );

                    _taskDirectoryManipulationService.CopyTaskFromPickupToRunFolder(task);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("{@action}{@taskfile}{@exception}",
                        "TaskAssembly loaded failed, multipl possible reasons",
                        task.Domain.BaseDirectory,
                        ex.ToString());
                    _taskDirectoryManipulationService.MoveTaskFromPickupToErrorFolder(task);
                }
            }
        }

        private void HandleUnloadableAssemblies(IEnumerable<TaskInformation> closableTasks)
        {
            foreach (var task in closableTasks)
            {
                if (_activeAssemblies.ContainsKey(task.Hash))
                {
                    _logger.LogInformation("{@action}{@task}",
                        "Removing task",
                        task.Domain.BaseDirectory);
                    _activeAssemblies.Remove(task.Hash);
                    _closedAssemblies.Add(task.Hash, task);
                }
                _taskDirectoryManipulationService.MoveTaskFromRunToUnloadFolder(task);
            }

        }

        private void ExecuteClasses()
        {
            foreach (var sharpTaskClassInformation in _activeTaskClasses)
            {
                var executeResult = _taskExecutionService.ShouldExecuteNow(sharpTaskClassInformation.Value, DateTime.Now);


                if (executeResult.ShouldExecuteNow)
                {
                    _logger.LogInformation("{@action}{@class}{@trigger}",
                        "Executing task",
                        sharpTaskClassInformation.Value.GetType(),
                        executeResult?.UsedTrigger?.GetType());

                    _taskExecutionService.MarkAsStarted(sharpTaskClassInformation.Value, DateTime.Now);
                    System.Threading.Thread runTask = new System.Threading.Thread(RunTask);
                    runTask.Start(sharpTaskClassInformation);
                }
            }
        }

        public void RunTask(object task)
        {
            var sharpTask = task as TaskClassState;
            if (sharpTask == null) return;

            _logger.LogInformation("{@action}{@class}{@name}{@description}",
                "Starting execution on seperate thread",
                sharpTask.GetType(),
                sharpTask.SharpTask.Name,
                sharpTask.SharpTask.Description);

            try
            {
                var result = sharpTask.SharpTask.RunTask(null);

                _logger.LogInformation("{@action}{@class}{@name}{@description}{@Successful}{@TaskFinished}",
                    "Execution ended",
                    sharpTask.GetType(),
                    sharpTask.SharpTask.Name,
                    sharpTask.SharpTask.Description,
                    result.Successful,
                    result.TaskFinished);

                _taskExecutionService.MarkAsFinishedOk(sharpTask, DateTime.Now);

                foreach (var log in result.LogLines)
                {
                    _logger.LogInformation("{@action}{@class}{@name}{@log}",
                        "Class log line",
                        sharpTask.GetType(),
                        sharpTask.SharpTask.Name,
                        log);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("{@action}{@class}{@name}{@description}{@excpetion}",
                    "Execution of tasl fails",
                    sharpTask.GetType(),
                    sharpTask.SharpTask.Name,
                    sharpTask.SharpTask.Description,
                    ex.ToString());
                _taskExecutionService.MarkAsFinishedError(sharpTask, DateTime.Now);

            }
        }
    }
}
