using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpTask.Core.Services.TaskCollection
{
    /// <summary>
    /// Manipulates the directories and move module files around, using appropriate related services
    /// </summary>
    /// 
    public interface ITaskCollectionService
    {
        Task<IEnumerator<object>> GetRunnableTask();

        Task<IEnumerator<object>> SynchronizeDirectories();
    }
}
