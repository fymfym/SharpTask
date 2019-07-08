using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Services.TaskDirectoryManipulation;
using SharpTask.Core.Services.TaskDllLoader;

namespace SharpTask.Core.Services.TaskDirectoryFileService
{
    public class TaskDirectoryFileService : IAssemblyCollectionService
    {
        private readonly ILogger<TaskDirectoryFileService> _logger;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;
        private readonly ITaskDllLoaderService _taskDllLoaderService;

        public TaskDirectoryFileService(
            ILogger<TaskDirectoryFileService> logger,
            ITaskDirectoryManipulationService taskDirectoryManipulationService,
            ITaskDllLoaderService taskDllLoaderService
            )
        {
            _logger = logger;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;
            _taskDllLoaderService = taskDllLoaderService;
        }


        public async Task<IEnumerable<TaskInformation>> GetRunnableTaskDirectories()
        {
            _logger.LogInformation("GetRunnableTasks");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            return await Task.Run(() => runnableTasks);
        }

        public async Task<IEnumerable<TaskInformation>> GetUnloadableTaskDirectories()
        {
            _logger.LogInformation("GetUnloadableTasks");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = runnableTasks.Where(rt => pickupTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable);
        }

        public async Task<IEnumerable<TaskInformation>> GetNewTaskDirectories()
        {
            var result = new List<TaskInformation>();
            _logger.LogInformation("GetNewTasks");
            var pickupTasks = await Task.Run(() => _taskDirectoryManipulationService.GetTasksInPickupFolder());

            foreach (var task in pickupTasks)
            {
                try
                {
                    var loadedAssembly = _taskDllLoaderService.LoadTaskIntoAppDomain(task);
                    result.Add(task);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(" {@action}{@assembly}{@exception}",
                        "GetNewTasks loading assembly failes",
                        task.PickupDirectory,
                        e.Message);
                }
            }
            return result;
        }
    }
}
