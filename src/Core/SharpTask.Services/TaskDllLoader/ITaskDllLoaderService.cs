using System;
using System.Collections.Generic;
using System.Text;
using SharpTask.Models;

namespace SharpTask.Core.Services.TaskDllLoader
{
    public interface ITaskDllLoaderService
    {
        object LoadDll(TaskModuleInformation taskModule);
    }
}
