using System.Collections.Generic;

namespace SharpTask.Core.Models.Task
{
    public interface ISharpTask
    {
        RunResult RunTask(TaskParameters taskParameters);
        List<ITriggerInterface> RunTrigger { get; }
        string Name { get; }
        string Description { get; }
        string Owner { get; }
    }
}
