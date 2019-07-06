using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public class TaskDllLoaderService : ITaskDllLoaderService
    {

        public Assembly LoadAssembly(AssemblyInformation assembly)
        {
            var myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assembly.FullFileName);

            var types = myAssembly.ExportedTypes;
            var classes = types.Where(x => x.IsClass).ToList();

            var result = new List<ISharpTask>();

            foreach (var cls in classes.Where(x => x.IsClass))
            {
                TypeInfo ti = (TypeInfo) cls;
                if (ti.ImplementedInterfaces.Any(x =>
                    x.FullName.Equals("SharpTask.Core.Models.Task.ISharpTask")))
                {
//                    var instance = Activator.CreateInstance(cls);
//                    result.Add(instance as ISharpTask);
                    return myAssembly;
                }
            }
            throw new Exception("Assembly does not contain implementation of interface <SharpTask.Core.Models.Task.ISharpTask>");
        }
    }
}
