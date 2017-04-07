using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class WeekdayTrigger : TaskTriggerInterface
    {

        DateTime _triggerDateTime;
        List<DayOfWeek> _weekdayList;

        public WeekdayTrigger(DateTime TriggerTime, List<DayOfWeek> WeekdayList)
        {
            _weekdayList = WeekdayList;
            _triggerDateTime = TriggerTime;
        }

        public string Description
        {
            get
            {
                return "Starts from StartDateTime date and every at marked weekday at StartDateTime time";
            }
        }

        public string Name
        {
            get
            {
                return "Executes at weekdays";
            }
        }


        public int Sequence { get; set; }

        public DateTime StartDateTime { get; set; }

        string TaskTriggerInterface.Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        string TaskTriggerInterface.Description
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DateTime GetTimeOnly(DateTime DateTime)
        {
            return new DateTime(1, 1, 1, DateTime.Hour, DateTime.Minute, DateTime.Second);
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {
            var ts = new TimeSpan(CurrentTime.Ticks - _triggerDateTime.Ticks).TotalSeconds;

            var day = CurrentTime.DayOfWeek;
            if (ts < 0) return false;
            if (_weekdayList.Count(x => x == day) < 1) return false;

            ts = new TimeSpan(GetTimeOnly(CurrentTime).Ticks - GetTimeOnly(_triggerDateTime).Ticks).TotalSeconds;

            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
