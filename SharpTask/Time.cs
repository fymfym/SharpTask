using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class Time
    {
        int _hour;
        int _minute;
        int _second;

        public Time()
        {

        }
        public Time(int Hour, int Minute, int Second)
        {
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
        }

        public override string ToString()
        {
            return DateTimeObject.ToString("HH:mm:ss");
        }

        public Time(DateTime DateTimeVaue)
        {
            this.Hour = DateTimeVaue.Hour;
            this.Minute = DateTimeVaue.Minute;
            this.Second = DateTimeVaue.Second;
        }
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Hour must be between 0 and 23");
                if (value > 23) throw new ArgumentOutOfRangeException("Hour must be between 0 and 23");
                _hour = value;
            }
        }

        public int Minute
        {
            get
            {
                return _second;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Second must be between 0 and 23");
                if (value > 39) throw new ArgumentOutOfRangeException("Second must be between 0 and 59");
                _second = value;
            }
        }

        public int Second
        {
            get
            {
                return _minute;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Minute must be between 0 and 23");
                if (value > 39) throw new ArgumentOutOfRangeException("Minute  must be between 0 and 59");
                _minute = value;
            }
        }

        public DateTime DateTimeObject
        {
            get
            {
                return new DateTime(1, 1, 1, _hour, _minute, _second);
            }
        }

        public long Ticks
        {
            get
            {
                return DateTimeObject.Ticks;
            }
        }
    }
}
