using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public interface ITaskDirectoryManipulationService
    {
        IEnumerable<AssemblyInformation> GetTasksInPickupFolder();
        IEnumerable<AssemblyInformation> GetTasksInRunFolder();

        Task<AssemblyInformation> CopyTaskFromPickupToRunFolder(AssemblyInformation taskInformation);
        Task<AssemblyInformation> MoveTaskFromPickupToErrorFolder(AssemblyInformation taskInformation);
        Task<AssemblyInformation> MoveTaskFromRunToUnloadFolder(AssemblyInformation taskInformation);
    }
}
