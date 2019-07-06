using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Services.TaskCollection;
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
        private Dictionary<long, AssemblyInformation> _activeAssemblies;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<long, AssemblyInformation> _closedAssemblies;
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

            _activeAssemblies = new Dictionary<long, AssemblyInformation>();
            _activeTaskClasses = new Dictionary<long, TaskClassState>();
            _closedAssemblies = new Dictionary<long, AssemblyInformation>();
        }

        public void Start()
        {
            _logger.LogInformation("Starting SharpTaskExecuter");
            _running = true;

            while (_running)
            {

                var unloadableAssembliesTask = _assemblyCollectionService.GetUnloadableAssemblies();
                var newAssembliesTask = _assemblyCollectionService.GetNewAssemblies();
                Task.WaitAll(unloadableAssembliesTask, newAssembliesTask);
                HandleUnloadableAssemblies(unloadableAssembliesTask.Result);
                HandleNewAssemblies(newAssembliesTask.Result);

                var runnableAssembliesTask = _assemblyCollectionService.GetRunnableAssemblies();
                Task.WaitAll(runnableAssembliesTask);
                LoadRunnableAssemblies(runnableAssembliesTask.Result);

                ExecuteClasses();
            }
        }

        public void Stop()
        {
            _running = false;
        }


        private void LoadRunnableAssemblies(IEnumerable<AssemblyInformation> runnableAssemblies)
        {
            foreach (var assemblyInformation in runnableAssemblies)
            {
                var assembly = _taskDllLoaderService.LoadAssembly(assemblyInformation);

                var classes = from exported in assembly.ExportedTypes
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

        private void HandleNewAssemblies(IEnumerable<AssemblyInformation> newAssemblies)
        {
            foreach (var assembly in newAssemblies)
            {
                if (_activeAssemblies.ContainsKey(assembly.Hash)) continue;
                _logger.LogInformation("{@action}{@task}",
                    "Adding tasks from pickup folder",
                    assembly.FullFileName);

                try
                {
                    _logger.LogInformation("{@action}{@assembly}",
                        "Load assembly",
                        assembly.FullFileName);

                    var loadedAssembly = _taskDllLoaderService.LoadAssembly(assembly);

                    _logger.LogInformation("{@action}{@assembly}{@class}",
                        "Loadeed assembly",
                        loadedAssembly.FullName,
                        loadedAssembly.GetType());

                    _activeAssemblies.Add(
                        assembly.Hash,
                        assembly
                        );

                    _taskDirectoryManipulationService.CopyTaskFromPickupToRunFolder(assembly);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("{@action}{@taskfile}{@exception}",
                        "TaskAssembly loaded failed, multipl possible reasons",
                        assembly.FullFileName,
                        ex.ToString());
                    _taskDirectoryManipulationService.MoveTaskFromPickupToErrorFolder(assembly);
                }
            }
        }

        private void HandleUnloadableAssemblies(IEnumerable<AssemblyInformation> closableAssemblies)
        {
            foreach (var assembly in closableAssemblies)
            {
                if (_activeAssemblies.ContainsKey(assembly.Hash))
                {
                    _logger.LogInformation("{@action}{@task}",
                        "Removing task",
                        assembly.FullFileName);
                    _activeAssemblies.Remove(assembly.Hash);
                    _closedAssemblies.Add(assembly.Hash, assembly);
                }
                _taskDirectoryManipulationService.MoveTaskFromRunToUnloadFolder(assembly);
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
                        executeResult.UsedTrigger.GetType());

                    _taskExecutionService.MarkAsStarted(sharpTaskClassInformation.Value,DateTime.Now);
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

                _taskExecutionService.MarkAsFinishedOk(sharpTask,DateTime.Now);

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
                _taskExecutionService.MarkAsFinishedError(sharpTask,DateTime.Now);

            }
        }
    }
}
