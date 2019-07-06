using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Services.TaskDirectoryManipulation;

namespace SharpTask.Core.Services.TaskCollection
{
    public class AssemblyCollectionService : IAssemblyCollectionService
    {
        private readonly ILogger<AssemblyCollectionService> _logger;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;

        public AssemblyCollectionService(
            ILogger<AssemblyCollectionService> logger,
            ITaskDirectoryManipulationService taskDirectoryManipulationService
            )
        {
            _logger = logger;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;
        }

        public async Task<IEnumerable<AssemblyInformation>> GetRunnableAssemblies()
        {
            _logger.LogInformation("GetRunnableAssemblies");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            return await Task.Run(() => runnableTasks);
        }

        public async Task<IEnumerable<AssemblyInformation>> GetUnloadableAssemblies()
        {
            _logger.LogInformation("GetUnloadableAssemblies");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = runnableTasks.Where(rt => pickupTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable);
        }

        public async Task<IEnumerable<AssemblyInformation>> GetNewAssemblies()
        {
            _logger.LogInformation("GetNewAssemblies");
            var runnableTasks = _taskDirectoryManipulationService.GetTasksInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetTasksInPickupFolder();

            var closable = pickupTasks.Where(rt => runnableTasks.All(pt => rt.Hash == pt.Hash));

            return await Task.Run(() => closable);
        }
    }
}
