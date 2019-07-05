using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Services.TaskCollection;
using SharpTask.Core.Services.TaskDirectoryManipulation;
using SharpTask.Core.Services.TaskDllLoader;

namespace SharpTask.Core.Services.TaskExecuter
{
    public class TaskExecuterService : ITaskExecuterService
    {
        private readonly ILogger<TaskExecuterService> _logger;
        private readonly ITaskCollectionService _taskCollectionService;
        private readonly ITaskDllLoaderService _taskDllLoaderService;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;

        bool _running;
        Dictionary<long, DllLoadState> _activeTasks;

        public TaskExecuterService(
            ILogger<TaskExecuterService> logger,
            ITaskCollectionService taskCollectionService,
            ITaskDllLoaderService taskDllLoaderService,
            ITaskDirectoryManipulationService taskDirectoryManipulationService
            )
        {
            _logger = logger;
            _taskCollectionService = taskCollectionService;
            _taskDllLoaderService = taskDllLoaderService;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;

            _activeTasks = new Dictionary<long, DllLoadState>();
        }

        public void Start()
        {
            _logger.LogInformation("Starting SharpTaskExecuter");
            _running = true;

            while (_running)
            {

                var closableTasksTask = _taskCollectionService.GetClosableTask();
                var runnableTasksTask = _taskCollectionService.GetRunnableTask();
                var newTasksTask = _taskCollectionService.GetNewTask();
                Task.WaitAll(closableTasksTask,runnableTasksTask, newTasksTask);
                var closableTasks = closableTasksTask.Result;
                var runnableTasks = runnableTasksTask.Result;
                var newTasks = newTasksTask.Result;

                foreach (var task in closableTasks)
                {
                    if (_activeTasks.ContainsKey(task.Hash))
                    {
                        _logger.LogInformation("{@action}{@task}",
                            "Removing task",
                            task.FullFileName);
                        _activeTasks[task.Hash].DisposeInstance();
                        _activeTasks.Remove(task.Hash);
                        _taskDirectoryManipulationService.MoveTaskFromRunToUnloadFolder(task);
                    }
                }

                foreach (var task in newTasks)
                {
                    if (_activeTasks.ContainsKey(task.Hash)) continue;
                    _logger.LogInformation("{@action}{@task}",
                        "Adding tasks from pickup folder",
                        task.FullFileName);


                    ISharpTask taskInstance;
                    try
                    {
                        _logger.LogInformation("{@action}{@task}",
                            "Instanciate task",
                            task.FullFileName);
                        taskInstance = _taskDllLoaderService.LoadDll(task);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("{@action}{@taskfile}{@exception}",
                            "TaskInstance not loaded",
                            task.FullFileName,
                            ex.ToString());
                        _taskDirectoryManipulationService.MoveTaskFromPickupToErrorFolder(task);
                        continue;
                    }

                    _activeTasks.Add(task.Hash, new DllLoadState(task, taskInstance));

                    _logger.LogInformation("{@action}{@task}",
                        "TaskInstance instance added",
                        task.FullFileName);
                    _taskDirectoryManipulationService.MoveTaskFromPickupToRunFolder(task);
                }

                foreach (var taskModuleInformation in runnableTasks)
                {
                    
                }
            }
        }

        public void Stop()
        {
            _running = false;
        }

        public void ExecuteDllLoadStateTasks()
        {
            _logger.LogInformation("Waiting for enqued tasks start trigger...");
            while (_running)
            {
                foreach (var task in _activeTasks)
                {
                    var now = DateTime.Now;
                    var res = task.Value.ShouldExecuteNow(now);
                    if (res.ShouldExecuteNow)
                    {
                        _logger.LogInformation($"Marking task {task.Value} as stared");
                        _logger.LogInformation(" - using triger {0}", res.UsedTrigger.Name ?? "Unnamed");
                        task.Value.MarkAsStarted(now);
                        System.Threading.Thread runTask = new System.Threading.Thread(_runTask);
                        runTask.Start(task.Value);
                    }
                }
                System.Threading.Thread.Sleep(1000);


//                if (new TimeSpan(DateTime.Now.Ticks - _lastDllLoad.Ticks).TotalSeconds > 30)
                {
                    //UpdateEnquedTasks();
                }
            }
            _logger.LogInformation("Enqued tasks stopped!");
        }

        void _runTask(object taskToRunObject)
        {
            DllLoadState taskToRun = (DllLoadState)taskToRunObject;
            _logger.LogInformation("Starting task: {0}", taskToRun.TaskInstance.GetType());

            var result = taskToRun.TaskInstance.RunTask(taskToRun.Parameters);
            if (result.TaskFinished)
            {
                if (result.Sucessfull)
                {
                    taskToRun.MarkAsFinishedOk(DateTime.Now);
                    _logger.LogInformation("Finished OK: {0}", taskToRun.TaskInstance.GetType());
                }
                else
                {
                    taskToRun.MarkAsFinishedError(DateTime.Now);
                    _logger.LogInformation("Finished with error: {0}", taskToRun.TaskInstance.GetType());
                }
            }
        }

    }

}
