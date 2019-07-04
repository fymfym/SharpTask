using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;
using Xunit;

namespace SharpTask.Core.Test.TaskTriggerTest
{

    public class TriggerWeekdayTest
    {
        private ITriggerInterface GetTriggerTime()
        {
            var wdl = new List<DayOfWeek>
            {
                DayOfWeek.Monday
            };
            return new TriggerWeekday(new StDate(2017,1,1), new StTime(12,0,0), wdl);
        }

        [Fact]
        public void OneDayTooEarlyTest()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0, 0); // Sunday
            var wd = GetTriggerTime();
            Assert.False(wd.ShouldRunNow(dt));
        }

        [Fact]
        public void OneDayTooLateTest()
        {
            var dt = new DateTime(2017, 1, 3, 12, 0, 0); // Tuesday
            var wd = GetTriggerTime();
            Assert.False(wd.ShouldRunNow(dt));
        }

        [Fact]
        public void SameDayTooLateTest()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 10); // Monday
            var wd = GetTriggerTime();
            Assert.False(wd.ShouldRunNow(dt));
        }

        [Fact]
        public void SameDayTwoBeforeTime()
        {
            var dt = new DateTime(2017, 1, 2, 11, 59, 59); // Monday
            var wd = GetTriggerTime();
            Assert.False(wd.ShouldRunNow(dt));
        }

        [Fact]
        public void SameDayAfterTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 10); // Monday
            var wd = GetTriggerTime();
            Assert.False(wd.ShouldRunNow(dt));
        }

        [Fact]
        public void SameDayJustBeforeEndTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 5); // Monday
            var wd = GetTriggerTime();
            Assert.True(wd.ShouldRunNow(dt));
        }

    }
}
