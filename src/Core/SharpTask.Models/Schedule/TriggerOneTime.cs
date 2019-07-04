using System;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Models.Schedule
{
    public class TriggerOneTime : ITriggerInterface
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Sequence { get; set; }

        public StDate TriggerDate { get; set; }

        public StTime TriggerTime { get; set; }

        public TriggerOneTime(StDate triggerDate, StTime triggerTime)
        {
            TriggerDate = triggerDate;
            TriggerTime = triggerTime;
            Name = "OneTimeTrigger";
            Description = "Executes at 'Triger date/ trigger time' only";
        }

        public override string ToString()
        {
            return TriggerDate != null ? $"Name: {Name} - TriggerOnce: {TriggerDate} {TriggerTime}" : "";
        }

        public bool ShouldRunNow(DateTime currentTime)
        {
            DateTime trigger = TriggerDate.DateTimeObject; // new DateTime(_triggerDate.Ticks + _triggerTime.Ticks);
            trigger = new DateTime(trigger.Ticks + TriggerTime.DateTimeObject.Ticks);
            
            var ts = new TimeSpan(currentTime.Ticks - trigger.Ticks).TotalSeconds;
            if (ts < 0) return false;
            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
