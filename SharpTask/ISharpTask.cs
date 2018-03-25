using System.Collections.Generic;

namespace SharpTask.Task
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
