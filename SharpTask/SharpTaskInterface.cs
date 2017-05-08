using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTask
{
    public interface ISharpTaskInterface
    {
        RunResult RunTask(TaskParameters taskParameters);
        List<SharpTask.TriggerInterface> RunTrigger { get; }
        string Name { get; }
        string Description { get; }
        string Owner { get; }
    }
}
