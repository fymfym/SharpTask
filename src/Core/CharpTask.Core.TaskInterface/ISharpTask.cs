using System.Collections.Generic;
using SharpTask.Core.Models;

namespace SharpTask.Core.TaskInterface
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
