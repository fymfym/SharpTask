using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public interface TaskTriggerInterface
    {
        string Name { get; set; }
        int CheckSequence { get; set; }
        bool ShouldRunNow(DateTime CurrentTime);
    }
}
