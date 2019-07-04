using System;

namespace SharpTask.Core.Models.Schedule
{
    public static class Helpers
    {
        public static DateTime GetTimeOnly(DateTime dateTime)
        {
            return new DateTime(1, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

    }
}
