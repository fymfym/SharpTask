using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class RepeatDailyTrigger : TaskTriggerInterface
    {
        string _name;
        string _description;
        int _sequence;

        List<DayOfWeek> _daysTorun;
        Date _triggerDate;
        List<Time> _startTimeList;

        public int Sequence
        {
            get
            {
                return _sequence;
            }

            set
            {
                _sequence = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public Date StartDate { get; set; }

        public RepeatDailyTrigger(Date StartDate, List<DayOfWeek> DaysTorun, List<Time> StartTimeList)
        {
            _daysTorun = DaysTorun;
            _triggerDate = StartDate;
            _startTimeList = StartTimeList;
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {
            var day = CurrentTime.DayOfWeek;
            if (_daysTorun.Count(x => x == day) < 1) return false;


            foreach (var t in _startTimeList)
            {
                var ts = new TimeSpan(Helpers.GetTimeOnly(CurrentTime).Ticks - t.Ticks).TotalSeconds;
                if ((ts >= 0) && (ts <= 5)) return true;
            }
            return false;
        }
    }
}
