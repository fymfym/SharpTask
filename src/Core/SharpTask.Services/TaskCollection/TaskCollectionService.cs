using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Services.TaskDirectoryManipulation;

namespace SharpTask.Core.Services.TaskCollection
{
    public class TaskCollectionService : ITaskCollectionService
    {
        private readonly ILogger<TaskCollectionService> _logger;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;

        public TaskCollectionService(
            ILogger<TaskCollectionService> logger,
            ITaskDirectoryManipulationService taskDirectoryManipulationService
            )
        {
            _logger = logger;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;
        }

        public async Task<IEnumerable<TaskModuleInformation>> GetRunnableTask()
        {
            _logger.LogInformation("GetRunnableTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            return await Task.Run(() => runnableTasks);
        }

        public async Task<IEnumerable<TaskModuleInformation>> GetUnloadbleTask()
        {
            _logger.LogInformation("GetUnloadbleTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = runnableTasks.Where(rt => pickupTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable);
        }

        public async Task<IEnumerable<TaskModuleInformation>> GetNewTask()
        {
            _logger.LogInformation("GetNewTask");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = pickupTasks.Where(rt => runnableTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable);
        }
    }
}
