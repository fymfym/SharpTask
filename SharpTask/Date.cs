using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class Date
    {
        int _year;
        int _month;
        int _day;

        public Date(int Year, int Month, int Day)
        {
            _year = Year;
            _month = Month;
            _day = Day;
        }

        public Date(DateTime DateTimeValue)
        {
            _year = DateTimeValue.Year;
            _month = DateTimeValue.Month;
            _day = DateTimeValue.Day;
        }

        public override string ToString()
        {
            return DateTimeObject.ToString("yyyy-MM-dd");
        }

        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Year must be beyween 0 and 9999");
                if (value > 9990) throw new ArgumentOutOfRangeException("Year must be beyween 0 and 9999");
                _year = value;
            }
        }

        public int Month
        {
            get
            {
                return _month;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Month must be beyween 1 and 12");
                if (value > 9990) throw new ArgumentOutOfRangeException("Month must be beyween 1 and 12");
                _month = value;
            }
        }

        public int Day
        {
            get
            {
                return _day;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Year must be beyween 0 and 9999");
                if (value > 31) throw new ArgumentOutOfRangeException("Year must be beyween 0 and 9999");
                if (value > 28)
                {
                    var dt = new DateTime(_year, _month, value);
                }
                _day = value;
            }
        }

        public DateTime DateTimeObject
        {
            get
            {
                return new DateTime(_year, _month, _day, 0, 0, 0);
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
