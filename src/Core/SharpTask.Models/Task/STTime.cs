using System;
// ReSharper disable NotResolvedInText
// ReSharper disable RedundantStringInterpolation

namespace SharpTask.Core.Models.Task
{
    public class StTime
    {
        int _hour;
        int _minute;
        int _second;

        public StTime(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public override string ToString()
        {
            return DateTimeObject.ToString("HH:mm:ss");
        }

        public StTime(DateTime dateTimeValue)
        {
            Hour = dateTimeValue.Hour;
            Minute = dateTimeValue.Minute;
            Second = dateTimeValue.Second;
        }
        public int Hour
        {
            get => _hour;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Hour must be between 0 and 23");
                if (value > 23) throw new ArgumentOutOfRangeException("Hour must be between 0 and 23");
                _hour = value;
            }
        }

        public int Minute
{
            get => _minute;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"Minute must be between 0 and 23");
                if (value > 59) throw new ArgumentOutOfRangeException($"Minute  must be between 0 and 59");
                _minute = value;
            }
        }

        public int Second
        {
            get => _second;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"Second must be between 0 and 23");
                if (value > 59) throw new ArgumentOutOfRangeException($"Second must be between 0 and 59");
                _second = value;
            }
        }

        public DateTime DateTimeObject => new DateTime(1, 1, 1, _hour, _minute, _second);

        public long Ticks => DateTimeObject.Ticks;
    }
}
