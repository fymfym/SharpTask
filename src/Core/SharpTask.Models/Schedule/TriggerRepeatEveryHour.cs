using System;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Models.Schedule
{
    public class TriggerRepeatEveryHour : ITriggerInterface
    {
        readonly int _repeatMinute;

        public int Sequence { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public StDate TriggerDate { get; set; }

        public StTime TriggerTime { get; set; }

        public TriggerRepeatEveryHour(int repeatMinute)
        {
            _repeatMinute = repeatMinute;
        }

        public bool ShouldRunNow(DateTime currentTime)
        {
            if (currentTime.Minute != _repeatMinute) return false;

            var ts = new TimeSpan(Helpers.GetTimeOnly(currentTime).Ticks - new StTime(currentTime.Hour,_repeatMinute,0).Ticks).TotalSeconds;
            if ((ts >= 0) && (ts <= 5))
            {
                TriggerTime = new StTime(currentTime);
                return true;
            }
            return false;
        }
    }
}
