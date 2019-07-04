using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Services.TaskDirectoryManipulation;
using SharpTask.Core.Services.TaskDllLoader;

namespace SharpTask.Core.Services.TaskCollection
{
    public class TaskCollectionService : ITaskCollectionService
    {
        private readonly ILogger<TaskCollectionService> _logger;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;
        private readonly ITaskDllLoaderService _taskDllLoaderService;

        public TaskCollectionService(
            ILogger<TaskCollectionService> logger,
            ITaskDirectoryManipulationService taskDirectoryManipulationService,
            ITaskDllLoaderService taskDllLoaderService
            )
        {
            _logger = logger;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;
            _taskDllLoaderService = taskDllLoaderService;
        }

        public async Task<IEnumerator<object>> GetRunnableTask()
        {
            var result = new List<object>();
            _logger.LogInformation("GetRunnableTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            foreach (var task in runnableTasks)
            {
                var obj = _taskDllLoaderService.LoadDll(task);
                result.Add(obj);
            }
            return await Task.Run(() => result.GetEnumerator());

        }

        public async Task<IEnumerator<object>> SynchronizeDirectories()
        {
            _logger.LogInformation("SynchronizeDirectories");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var loadableTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            foreach (var task in loadableTasks.Where(x => runnableTasks.All(y => x.Hash != y.Hash)))
            {
                await _taskDirectoryManipulationService.CopyTaskFromPickupToRunFolder(task);
            }

            return null;
        }
    }
}
