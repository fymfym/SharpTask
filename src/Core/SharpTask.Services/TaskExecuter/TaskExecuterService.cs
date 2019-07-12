using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        private Dictionary<string,TaskClassState> _activeTasks;

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

            _activeTasks = new Dictionary<string, TaskClassState>();
        }

        private List<TaskInformation> _oldRunnable = new List<TaskInformation>();

        public void Start()
        {
            _logger.LogInformation("Starting SharpTaskExecuter starting");
            _running = true;

            var unloadableTaskDirectoriesTask = _assemblyCollectionService.GetUnloadableTaskDirectories();
            Task.WaitAll(unloadableTaskDirectoriesTask);
            HandleUnloadable(unloadableTaskDirectoriesTask.Result);

            while (_running)
            {
                var newTaskDirectoriesTask = _assemblyCollectionService.GetNewTaskDirectories();
                Task.WaitAll(newTaskDirectoriesTask);
                HandleNewAssemblies(newTaskDirectoriesTask.Result);

                var runnableTaskDirectoriesTask = _assemblyCollectionService.GetRunnableTaskDirectories();
                Task.WaitAll(runnableTaskDirectoriesTask);

                List<TaskInformation> runnable = runnableTaskDirectoriesTask.Result.ToList();

                if (string.Join(',',_oldRunnable.Select(x => x.DirectoryMd5)) !=
                    string.Join(',' ,runnableTaskDirectoriesTask.Result.Select(x => x.DirectoryMd5)))
                {
                    var lst = from run in runnable
                                join old in _oldRunnable
                            on run.DirectoryMd5 equals old.DirectoryMd5
                        select run;

                    foreach (var taskInformation in lst)
                    {
                        _logger.LogInformation("{@action}{@tasks}",
                            "New task added",
                            taskInformation.Directory.Name);
                    }

                        
                        
                }

                LoadRunnableAssemblies(runnableTaskDirectoriesTask.Result);
                _oldRunnable = runnableTaskDirectoriesTask.Result.ToList();



                ExecuteClasses(runnableTaskDirectoriesTask.Result);
            }
        }

        public void Stop()
        {
            _running = false;
        }


        private void LoadRunnableAssemblies(IEnumerable<TaskInformation> taskInformation)
        {
            foreach (var task in taskInformation)
            {
                _taskDllLoaderService.LoadTaskIntoAppDomain(task);
            }
        }

        private void HandleUnloadable(IEnumerable<TaskInformation> taskInformation)
        {
            foreach (var task in taskInformation)
            {
                _taskDirectoryManipulationService.MoveDirectoryUnloadFolder(task);
            }
        }
        

        private void HandleNewAssemblies(IEnumerable<TaskInformation> taskInformation)
        {
            foreach (var task in taskInformation)
            {
                try
                {
                    _logger.LogInformation("{@action}{@directory}",
                        "Copying task to run folder",
                        task.Directory.FullName);

                    _taskDirectoryManipulationService.CopyDirectoryToRunFolder(task);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("{@action}{@taskfile}{@exception}",
                        "TaskAssembly loaded failed, multipl possible reasons",
                        task.Directory.FullName,
                        ex.ToString());
                    _taskDirectoryManipulationService.MoveDirectoryToErrorFolder(task);
                }
            }
        }

        private void ExecuteClasses(IEnumerable<TaskInformation> taskInformationList)
        {
            foreach (var taskInformation in taskInformationList)
            {
                foreach (var instance in taskInformation.RunInstance)
                {
                    var sharpTaskClassInformation = new TaskClassState(taskInformation, instance);
                 
                    if (_activeTasks.ContainsKey(taskInformation.DirectoryMd5)) continue;

                    _activeTasks.Add(taskInformation.DirectoryMd5, sharpTaskClassInformation);
                    
                    var executeResult = _taskExecutionService.ShouldExecuteNow(sharpTaskClassInformation, DateTime.Now);

                    if (executeResult.ShouldExecuteNow)
                    {
                        _logger.LogInformation("{@action}{@class}{@trigger}",
                            "Executing task",
                            sharpTaskClassInformation.GetType(),
                            executeResult.UsedTrigger?.GetType());

                        _taskExecutionService.MarkAsStarted(sharpTaskClassInformation, DateTime.Now);
                        System.Threading.Thread runTask = new System.Threading.Thread(RunTask);
                        runTask.Start(sharpTaskClassInformation);
                    }
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
