using System;
using SharpTask.Core.Models;

namespace SharpTask.Core.TaskInterface
{
    public interface ITriggerInterface
    {
        StDate TriggerDate { get; set; }
        StTime TriggerTime { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Sequence { get; set; }
        bool ShouldRunNow(DateTime currentTime);
    }
}
