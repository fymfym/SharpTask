using System.Collections.Generic;
using SharpTask.Models;

namespace SharpTask.Services.TaskDirectoryManipulation
{
    public interface ITaskManipulationService
    {
        IEnumerable<TaskModuleInformation> GetPickupTasks();
        IEnumerable<TaskModuleInformation> GetRunTasks();

    }
}
