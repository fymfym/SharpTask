using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public interface ITaskDllLoaderService
    {
        ISharpTask LoadDll(TaskModuleInformation taskModule);
    }
}
