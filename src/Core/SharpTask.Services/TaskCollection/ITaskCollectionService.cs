using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskCollection
{
    /// <summary>
    /// Manipulates the directories and move module files around, using appropriate related services
    /// </summary>
    /// 
    public interface ITaskCollectionService
    {
        Task<IEnumerable<TaskModuleInformation>> GetRunnableTask();
        Task<IEnumerable<TaskModuleInformation>> GetClosableTask();
        Task<IEnumerable<TaskModuleInformation>> GetNewTask();
    }
}
