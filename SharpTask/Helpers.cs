using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTask
{
    public static class Helpers
    {
        public static DateTime GetTimeOnly(DateTime DateTime)
        {
            return new DateTime(1, 1, 1, DateTime.Hour, DateTime.Minute, DateTime.Second);
        }

    }
}
