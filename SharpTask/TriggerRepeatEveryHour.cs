using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTask
{
    public class TriggerRepeatEveryHour : TriggerInterface
    {

        string _name;
        string _description;
        int _sequence;

        Date _triggerDate;
        Time _triggerTime;
        int _repeatMinute;

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

        public TriggerRepeatEveryHour(Date StartDate, int RepeatMinute)
        {
            _repeatMinute = RepeatMinute;
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {
            if (CurrentTime.Minute != _repeatMinute) return false;

            var ts = new TimeSpan(Helpers.GetTimeOnly(CurrentTime).Ticks - new Time(CurrentTime.Hour,_repeatMinute,0).Ticks).TotalSeconds;
            if ((ts >= 0) && (ts <= 5))
            {
                _triggerTime = new SharpTask.Time(CurrentTime);
                return true;
            }
            return false;
        }
    }
}
