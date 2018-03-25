using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTask.Task
{
    public interface ITriggerInterface
    {
        STDate TriggerDate { get; set; }
        STTime TriggerTime { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Sequence { get; set; }
        bool ShouldRunNow(DateTime CurrentTime);
    }
}
