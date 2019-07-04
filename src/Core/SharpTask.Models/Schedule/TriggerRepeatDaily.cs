using System;
using System.Collections.Generic;
using System.Linq;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Models.Schedule
{
    public class TriggerRepeatDaily : ITriggerInterface
    {
        readonly List<DayOfWeek> _daysTorun;
        readonly StTime _startTime;

        public int Sequence { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public StDate TriggerDate { get; set; }

        public StTime TriggerTime { get; set; }

        public TriggerRepeatDaily(StDate startDate, List<DayOfWeek> daysTorun, StTime startTime)
        {
            _daysTorun = daysTorun;
            TriggerDate = startDate;
            _startTime = startTime;
        }

        public bool ShouldRunNow(DateTime currentTime)
        {

            var day = currentTime.DayOfWeek;
            if (_daysTorun.Count(x => x == day) < 1) return false;

            var ts = new TimeSpan(Helpers.GetTimeOnly(currentTime).Ticks - _startTime.Ticks).TotalSeconds;
            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
