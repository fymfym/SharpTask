using System;

namespace SharpTask.Core.Models.Task
{
    public enum ExecutionResult { NotSet, Ok, Error };

    public class TaskClassExecution
    {
        public ExecutionResult ExecutionResult { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}
