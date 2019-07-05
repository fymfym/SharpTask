using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;
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
        Dictionary<long, AssemblyLibraryState> _activeTasks;

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

            _activeTasks = new Dictionary<long, AssemblyLibraryState>();
        }

        public void Start()
        {
            _logger.LogInformation("Starting SharpTaskExecuter");
            _running = true;

            while (_running)
            {

                var unloadableTaskTasks = _taskCollectionService.GetUnloadbleTask();
                var newTasksTask = _taskCollectionService.GetNewTask();
                Task.WaitAll(unloadableTaskTasks, newTasksTask);
                HandleUnloadableTasks(unloadableTaskTasks.Result);
                HandleNewTasks(newTasksTask.Result);

                var runnableTasksTask = _taskCollectionService.GetRunnableTask();
                Task.WaitAll(runnableTasksTask);
                HandleRunnableTasks(runnableTasksTask.Result);
            }
        }

        public void Stop()
        {
            _running = false;
        }

        private void HandleRunnableTasks(IEnumerable<TaskModuleInformation> runnableTasks)
        {
            foreach (var taskModuleInformation in runnableTasks)
            {
                var assembly = _taskDllLoaderService.LoadAssembly(taskModuleInformation);

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
                        var result = instance.RunTask(null);
                    }
                }

            }
        }

        private void HandleNewTasks(IEnumerable<TaskModuleInformation> newTasks)
        {
            foreach (var task in newTasks)
            {
                if (_activeTasks.ContainsKey(task.Hash)) continue;
                _logger.LogInformation("{@action}{@task}",
                    "Adding tasks from pickup folder",
                    task.FullFileName);

                try
                {
                    _logger.LogInformation("{@action}{@task}",
                        "Instanciate task",
                        task.FullFileName);
                    var assembly = _taskDllLoaderService.LoadAssembly(task);

                    _logger.LogInformation("{@action}{@task}{@class}",
                        "TaskAssembly instance added",
                        assembly.FullName,
                        assembly.GetType());
                    _activeTasks.Add(
                        task.Hash,
                        new AssemblyLibraryState(
                            task,
                            assembly
                        )
                    );
                    _taskDirectoryManipulationService.CopyTaskFromPickupToRunFolder(task);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("{@action}{@taskfile}{@exception}",
                        "TaskAssembly loaded failed, multipl possible reasons",
                        task.FullFileName,
                        ex.ToString());
                    _taskDirectoryManipulationService.MoveTaskFromPickupToErrorFolder(task);
                    continue;
                }

            }
        }

        private void HandleUnloadableTasks(IEnumerable<TaskModuleInformation> closableTasks)
        {
            foreach (var task in closableTasks)
            {
                if (_activeTasks.ContainsKey(task.Hash))
                {
                    _logger.LogInformation("{@action}{@task}",
                        "Removing task",
                        task.FullFileName);
                    _activeTasks[task.Hash].DisposeAssembly();
                    _activeTasks.Remove(task.Hash);
                }
                _taskDirectoryManipulationService.MoveTaskFromRunToUnloadFolder(task);
            }

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
            AssemblyLibraryState taskToRun = (AssemblyLibraryState)taskToRunObject;
            _logger.LogInformation("Starting task: {0}", taskToRun.TaskAssembly.GetType());

            //var result = taskToRun.TaskAssembly.RunTask(taskToRun.Parameters);
            //if (result.TaskFinished)
            //{
            //    if (result.Sucessfull)
            //    {
            //        taskToRun.MarkAsFinishedOk(DateTime.Now);
            //        _logger.LogInformation("Finished OK: {0}", taskToRun.TaskAssembly.GetType());
            //    }
            //    else
            //    {
            //        taskToRun.MarkAsFinishedError(DateTime.Now);
            //        _logger.LogInformation("Finished with error: {0}", taskToRun.TaskAssembly.GetType());
            //    }
            //}
        }

    }

}
