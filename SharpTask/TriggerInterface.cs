using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public interface TriggerInterface
    {
        Date TriggerDate { get; set; }
        Time TriggerTime { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Sequence { get; set; }
        bool ShouldRunNow(DateTime CurrentTime);
    }
}
