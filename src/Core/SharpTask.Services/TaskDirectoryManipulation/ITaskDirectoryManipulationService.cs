using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public interface ITaskDirectoryManipulationService
    {
        IEnumerable<TaskInformation> GetDirectoriesInPickupFolder();
        IEnumerable<TaskInformation> GetDirectoriesInRunFolder();

        Task<TaskInformation> CopyDirectoryToRunFolder(TaskInformation taskInformation);
        Task<TaskInformation> MoveDirectoryToErrorFolder(TaskInformation taskInformation);
        Task<TaskInformation> MoveDirectoryUnloadFolder(TaskInformation taskInformation);
    }
}
