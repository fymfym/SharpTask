using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskExecuterTest
{

    [TestFixture]
    public class RepeatDailyTriggerTest
    {
 
        SharpTaskTask.TaskTriggerInterface GetTriggerTime()
        {
            var dtr = new List<DayOfWeek>();
            dtr.Add(DayOfWeek.Monday);
            dtr.Add(DayOfWeek.Friday);
            var ttr = new List<SharpTaskTask.Time>();
            ttr.Add(new SharpTaskTask.Time(12, 0, 0));
            ttr.Add(new SharpTaskTask.Time(15, 0, 0));
            return new SharpTaskTask.RepeatDailyTrigger(new SharpTaskTask.Date(2017, 1, 1), dtr, ttr);
        }

        [Test]
        public void OneDayTooEarlyTest()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0, 0); // Sunday
            var wd = GetTriggerTime();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void OneDayTooLateTest()
        {
            var dt = new DateTime(2017, 1, 3, 12, 0, 0); // Tuesday
            var wd = GetTriggerTime();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SameDayTooLateTest()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 10); // Monday
            var wd = GetTriggerTime();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SameDayAfterTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 10); // Monday
            var wd = GetTriggerTime();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SameDayJustBeforeEndTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 5); // Monday
            var wd = GetTriggerTime();
            Assert.IsTrue(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SecondDayOf()
        {
            var dt = new DateTime(2017, 1, 6, 12, 0, 2); // Friday
            var wd = GetTriggerTime();
            Assert.IsTrue(wd.ShouldRunNow(dt));
        }

    }
}
