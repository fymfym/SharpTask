using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryFileService
{
    /// <summary>
    /// Manipulates the directories and move module files around, using appropriate related services
    /// </summary>
    /// 
    public interface IAssemblyCollectionService
    {
        Task<IEnumerable<TaskInformation>> GetRunnableTaskDirectories();
        Task<IEnumerable<TaskInformation>> GetUnloadableTaskDirectories();
        Task<IEnumerable<TaskInformation>> GetNewTaskDirectories();
    }
}
