using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class WeekdayTrigger : TaskTriggerInterface
    {

        Date _triggerDate;
        Time _triggerTime;
        List<DayOfWeek> _weekdayList;

        /// <summary>
        /// Starts from StartDateTime date and every at marked weekday at StartDateTime time
        /// </summary>
        public WeekdayTrigger()
        {

        }

        public WeekdayTrigger(Date Startdate, Time ExecuteTime, List<DayOfWeek> WeekdayList)
        {
            _weekdayList = WeekdayList;
            _triggerDate = Startdate;
            _triggerTime = ExecuteTime;
        }

        public int Sequence { get; set; }

        public Date TriggerDate
        {
            get
            {
                return _triggerDate;
            }
            set
            {
                _triggerDate = value;
            }
        }

        public Time TriggerTime
        {
            get
            {
                return _triggerTime;
            }
            set
            {
                _triggerTime = value;
            }
        }


        string TaskTriggerInterface.Name { get; set; }

        string TaskTriggerInterface.Description { get; set; }


        public bool ShouldRunNow(DateTime CurrentTime)
        {
            var ts = new TimeSpan(Helpers.GetTimeOnly(CurrentTime).Ticks - _triggerTime.Ticks).TotalSeconds;

            var day = CurrentTime.DayOfWeek;
            if (ts < 0) return false;
            if (_weekdayList.Count(x => x == day) < 1) return false;

            ts = new TimeSpan(Helpers.GetTimeOnly(CurrentTime).Ticks - _triggerTime.Ticks).TotalSeconds;

            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
