using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.TaskModule;
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

        public async Task<IEnumerable<TaskModuleInformation>> GetRunnableTask()
        {
            var result = new List<TaskModuleInformation>();
            _logger.LogInformation("GetRunnableTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            return await Task.Run(() => runnableTasks);
        }

        public async Task<IEnumerator<TaskModuleInformation>> GetClosableTask()
        {
            var result = new List<object>();
            _logger.LogInformation("GetRunnableTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = runnableTasks.Where(rt => pickupTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable.GetEnumerator());
        }

        public async Task<IEnumerator<TaskModuleInformation>> GetNewTask()
        {
            var result = new List<object>();
            _logger.LogInformation("GetRunnableTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = pickupTasks.Where(rt => runnableTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable.GetEnumerator());
        }
    }
}
