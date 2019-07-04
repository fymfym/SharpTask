using System;
using System.Collections.Generic;
using System.Linq;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Models.Schedule
{
    public class TriggerWeekday : ITriggerInterface
    {
        readonly List<DayOfWeek> _weekdayList;

        /// <summary>
        /// Starts from StartDateTime date and every at marked weekday at StartDateTime time
        /// </summary>
        public TriggerWeekday()
        {

        }

        public TriggerWeekday(StDate startdate, StTime executeTime, List<DayOfWeek> weekdayList)
        {
            _weekdayList = weekdayList;
            TriggerDate = startdate;
            TriggerTime = executeTime;
        }

        public int Sequence { get; set; }

        public StDate TriggerDate { get; set; }

        public StTime TriggerTime { get; set; }


        string ITriggerInterface.Name { get; set; }

        string ITriggerInterface.Description { get; set; }


        public bool ShouldRunNow(DateTime currentTime)
        {
            var ts = new TimeSpan(Helpers.GetTimeOnly(currentTime).Ticks - TriggerTime.Ticks).TotalSeconds;

            var day = currentTime.DayOfWeek;
            if (ts < 0) return false;
            if (_weekdayList.Count(x => x == day) < 1) return false;

            ts = new TimeSpan(Helpers.GetTimeOnly(currentTime).Ticks - TriggerTime.Ticks).TotalSeconds;

            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
