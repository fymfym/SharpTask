using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public interface ITaskDirectoryManipulationService
    {
        IEnumerable<TaskInformation> GetTasksInPickupFolder();
        IEnumerable<TaskInformation> GetTasksInRunFolder();

        Task<TaskInformation> CopyTaskFromPickupToRunFolder(TaskInformation taskInformation);
        Task<TaskInformation> MoveTaskFromPickupToErrorFolder(TaskInformation taskInformation);
        Task<TaskInformation> MoveTaskFromRunToUnloadFolder(TaskInformation taskInformation);
    }
}
