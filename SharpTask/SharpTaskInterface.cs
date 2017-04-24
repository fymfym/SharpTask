using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public interface SharpTaskInterface
    {
        RunResult RunTask(TaskParameters taskParameters);
        List<SharpTaskTask.TriggerInterface> RunTrigger { get; }
        string Name { get; }
        string Description { get; }
        string Owner { get; }
    }
}
