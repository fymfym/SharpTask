using System.Collections.Generic;
using System.Threading.Tasks;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskCollection
{
    /// <summary>
    /// Manipulates the directories and move module files around, using appropriate related services
    /// </summary>
    /// 
    public interface IAssemblyCollectionService
    {
        Task<IEnumerable<AssemblyInformation>> GetRunnableAssemblies();
        Task<IEnumerable<AssemblyInformation>> GetUnloadableAssemblies();
        Task<IEnumerable<AssemblyInformation>> GetNewAssemblies();
    }
}
