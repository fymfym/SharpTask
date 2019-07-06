using System.Reflection;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public interface ITaskDllLoaderService
    {
        Assembly LoadAssembly(AssemblyInformation assembly);
    }
}
