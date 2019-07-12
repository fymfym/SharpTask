using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public class TaskDllLoaderService : ITaskDllLoaderService
    {

        private readonly ILogger<TaskDllLoaderService> _logger;

        public TaskDllLoaderService(
            ILogger<TaskDllLoaderService> logger
            )
        {
            _logger = logger;
        }

        public TaskInformation LoadTaskIntoAppDomain(TaskInformation task)
        {
            var result = new List<ISharpTask>();

            foreach (var file in task.Directory.GetFiles())
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFile(file.FullName);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("{@action}{@filename}{exception}",
                        "Assembly.LoadFile failes",
                        file.FullName,
                        ex
                        );
                }

                if (assembly == null) continue;

                var types = assembly.ExportedTypes;
                var classes = types.Where(x => x.IsClass).ToList();


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
            }
            task.RunInstance = result;
            return task;
        }
    }
}
