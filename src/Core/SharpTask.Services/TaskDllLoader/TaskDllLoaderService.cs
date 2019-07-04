using System;
using System.IO;
using System.Runtime.Loader;
using SharpTask.Models;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public class TaskDllLoaderService : ITaskDllLoaderService
    {

        public object LoadDll(TaskModuleInformation taskModule)
        {
            var myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(taskModule.FullFileName);
            var myType = myAssembly.GetType("SharpTask.Core.Models.Task.ISharpTask");
            var myInstance = Activator.CreateInstance(myType);
            return myInstance;
        }
    }
}
