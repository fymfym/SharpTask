using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public class TaskDllLoaderService : ITaskDllLoaderService
    {

        public TaskInformation LoadTaskIntoAppDomain(TaskInformation task)
        {

            var domain = AppDomain.CreateDomain(task.TaskDirectoryName);

            var dir = new DirectoryInfo(task.RunDirectory);

            foreach (var file in dir.GetFiles())
            {
                var assembly = domain.Load(file.FullName);

                var types = assembly.ExportedTypes;
                var classes = types.Where(x => x.IsClass).ToList();

                var result = new List<ISharpTask>();

                foreach (var cls in classes.Where(x => x.IsClass))
                {
                    TypeInfo ti = (TypeInfo)cls;
                    if (ti.ImplementedInterfaces.Any(x =>
                        x.FullName.Equals("SharpTask.Core.Models.Task.ISharpTask")))
                    {
                        var instance = Activator.CreateInstance(cls);
                        result.Add(instance as ISharpTask);
                    }
                }
                task.RunInstance = result;
            }
            task.Domain = domain;
            return task;
        }
    }
}
