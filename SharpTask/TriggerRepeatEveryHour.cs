using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharpTask.Task
{
    public class TriggerRepeatEveryHour : ITriggerInterface
    {

        string _name;
        string _description;
        int _sequence;

        STDate _triggerDate;
        STTime _triggerTime;
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

        public STDate TriggerDate
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

        public STTime TriggerTime
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

        public TriggerRepeatEveryHour(STDate StartDate, int RepeatMinute)
        {
            _repeatMinute = RepeatMinute;
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {
            if (CurrentTime.Minute != _repeatMinute) return false;

            var ts = new TimeSpan(Helpers.GetTimeOnly(CurrentTime).Ticks - new STTime(CurrentTime.Hour,_repeatMinute,0).Ticks).TotalSeconds;
            if ((ts >= 0) && (ts <= 5))
            {
                _triggerTime = new STTime(CurrentTime);
                return true;
            }
            return false;
        }
    }
}
