using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
