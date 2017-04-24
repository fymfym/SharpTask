using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class TriggerRepeatDaily : TriggerInterface
    {

        string _name;
        string _description;
        int _sequence;

        List<DayOfWeek> _daysTorun;
        Date _triggerDate;
        Time _triggerTime;
        Time _startTime;

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

        public TriggerRepeatDaily(Date StartDate, List<DayOfWeek> DaysTorun, Time StartTime)
        {
            _daysTorun = DaysTorun;
            _triggerDate = StartDate;
            _startTime = StartTime;
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {

            var day = CurrentTime.DayOfWeek;
            if (_daysTorun.Count(x => x == day) < 1) return false;

            var ts = new TimeSpan(Helpers.GetTimeOnly(CurrentTime).Ticks - _startTime.Ticks).TotalSeconds;
            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
