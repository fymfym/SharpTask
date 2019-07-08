using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Models.TaskModule
{
    public class TaskInformation
    {
        public string TaskDirectoryName;
        public string PickupDirectory;
        public string RunDirectory;
        public AppDomain Domain;
        public IEnumerable<ISharpTask> RunInstance;
        public long Hash;
    }
}
