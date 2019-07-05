using System;
using System.IO;
using System.Runtime.Loader;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public class TaskDllLoaderService : ITaskDllLoaderService
    {

        public ISharpTask LoadDll(TaskModuleInformation taskModule)
        {
            var myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(taskModule.FullFileName);
            var myType = myAssembly.GetType("SharpTask.Core.Models.TaskInstance.ISharpTask");
            var myInstance = Activator.CreateInstance(myType);
            return (ISharpTask)myInstance;
        }
    }
}
