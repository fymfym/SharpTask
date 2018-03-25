using System;

namespace SharpTask.Task
{
    public class StDate
    {
        int _year;
        int _month;
        int _day;

        public StDate(int year, int month, int day)
        {
            _year = year;
            _month = month;
            _day = day;
        }

        public StDate(DateTime dateTimeValue)
        {
            _year = dateTimeValue.Year;
            _month = dateTimeValue.Month;
            _day = dateTimeValue.Day;
        }

        public override string ToString()
        {
            return DateTimeObject.ToString("yyyy-MM-dd");
        }

        public int Year
        {
            get => _year;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($@"Year must be beyween 0 and 9999");
                if (value > 9990) throw new ArgumentOutOfRangeException($@"Year must be beyween 0 and 9999");
                _year = value;
            }
        }

        public int Month
        {
            get => _month;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"Month must be beyween 1 and 12");
                if (value > 9990) throw new ArgumentOutOfRangeException($"Month must be beyween 1 and 12");
                _month = value;
            }
        }

        public int Day
        {
            get => _day;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"Day must be beyween 1 and 28/29/30/31");
                if (value > 31) throw new ArgumentOutOfRangeException($"Day  must be beyween 1 and 28/29/30/31");
                if (value > 28)
                {
                    var dt = new DateTime(_year, _month, value);
                    if (dt.Day != value) throw new ArgumentOutOfRangeException($"Day of month is not valid");
                }
                _day = value;
            }
        }

        public DateTime DateTimeObject => new DateTime(_year, _month, _day, 0, 0, 0);

        public long Ticks => DateTimeObject.Ticks;
    }
}
