using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{

    [TestFixture]
    public class TriggerWeekdayTest
    {
 
        SharpTaskTask.TriggerInterface GetTriggerTime()
        {
            var wdl = new List<DayOfWeek>();
            wdl.Add(DayOfWeek.Monday);
            return new SharpTaskTask.TriggerWeekday(new SharpTaskTask.Date(2017,1,1), new SharpTaskTask.Time(12,0,0), wdl);
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
        public void SameDayTwoBeforeTime()
        {
            var dt = new DateTime(2017, 1, 2, 11, 59, 59); // Monday
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

    }
}
