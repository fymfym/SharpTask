using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Services.TaskDirectoryManipulation;

namespace SharpTask.Core.Services.TaskDirectoryFileService
{
    public class TaskDirectoryFileService : IAssemblyCollectionService
    {
        private readonly ILogger<TaskDirectoryFileService> _logger;
        private readonly ITaskDirectoryManipulationService _taskDirectoryManipulationService;

        public TaskDirectoryFileService(
            ILogger<TaskDirectoryFileService> logger,
            ITaskDirectoryManipulationService taskDirectoryManipulationService
            )
        {
            _logger = logger;
            _taskDirectoryManipulationService = taskDirectoryManipulationService;
        }

        public async Task<IEnumerable<TaskInformation>> GetRunnableTaskDirectories()
        {
            var runFolder = _taskDirectoryManipulationService.GetDirectoriesInRunFolder();
            var pickupFolder = _taskDirectoryManipulationService.GetDirectoriesInPickupFolder();

            var runnable = from run in runFolder
                           join pickup in pickupFolder
                               on run.DirectoryName equals pickup.DirectoryName
                           select run;

            return await Task.Run(() => runnable);
        }

        public async Task<IEnumerable<TaskInformation>> GetUnloadableTaskDirectories()
        {
            _logger.LogInformation("GetUnloadableTasks");
            var runnableTasks = _taskDirectoryManipulationService.GetDirectoriesInRunFolder();
            var pickupTasks = _taskDirectoryManipulationService.GetDirectoriesInPickupFolder();

            var closable = runnableTasks.Where(rt => pickupTasks.All(pt => rt.DirectoryMd5 == pt.DirectoryMd5));

            return await Task.Run(() => closable);
        }

        public async Task<IEnumerable<TaskInformation>> GetNewTaskDirectories()
        {
            var result = new List<TaskInformation>();
            var pickupTasks = await Task.Run(() => _taskDirectoryManipulationService.GetDirectoriesInPickupFolder().ToList());
            var runnableTasks = await Task.Run(() => _taskDirectoryManipulationService.GetDirectoriesInRunFolder().ToList());

            foreach (var task in pickupTasks)
            {
                var version = runnableTasks.FirstOrDefault(x => x.Directory.Name == task.DirectoryName);
                if (version == null)
                {
                    result.Add(task);
                }
            }
            return result;
        }
    }
}
