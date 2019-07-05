using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public interface ITaskDirectoryManipulationService
    {
        IEnumerable<TaskModuleInformation> GetTasksInPickupFolder();
        IEnumerable<TaskModuleInformation> GetTasksInRunFolder();

        Task<TaskModuleInformation> CopyTaskFromPickupToRunFolder(TaskModuleInformation taskInformation);
        Task<TaskModuleInformation> MoveTaskFromPickupToErrorFolder(TaskModuleInformation taskInformation);
        Task<TaskModuleInformation> MoveTaskFromRunToUnloadFolder(TaskModuleInformation taskInformation);
    }
}
