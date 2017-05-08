using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpTask;

namespace SharTaskTest.SharpTaskExecuterTest
{

    [TestFixture]
    public class TriggerRepeatDailyTest
    {
 
        SharpTask.TriggerInterface GetTriggerHour()
        {
            var dtr = new List<DayOfWeek>();
            dtr.Add(DayOfWeek.Monday);
            dtr.Add(DayOfWeek.Friday);
            return new SharpTask.TriggerRepeatDaily(new SharpTask.Date(2017, 1, 1), dtr, new SharpTask.Time(12,0,0));
        }

        SharpTask.TriggerInterface GetTriggerSeconds()
        {
            var dtr = new List<DayOfWeek>();
            dtr.Add(DayOfWeek.Monday);
            dtr.Add(DayOfWeek.Friday);
            return new SharpTask.TriggerRepeatDaily(new SharpTask.Date(2017, 1, 1), dtr, new Time(0,0,0) );
        }

        [Test]
        public void OneDayTooEarlyTest()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0, 0); // Sunday
            var wd = GetTriggerHour();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void OneDayTooLateTest()
        {
            var dt = new DateTime(2017, 1, 3, 12, 0, 0); // Tuesday
            var wd = GetTriggerHour();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SameDayTooLateTest()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 10); // Monday
            var wd = GetTriggerHour();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SameDayAfterTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 10); // Monday
            var wd = GetTriggerHour();
            Assert.IsFalse(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SameDayJustBeforeEndTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 0, 5); // Monday
            var wd = GetTriggerHour();
            Assert.IsTrue(wd.ShouldRunNow(dt));
        }

        [Test]
        public void SecondDayOf()
        {
            var dt = new DateTime(2017, 1, 6, 12, 0, 2); // Friday
            var wd = GetTriggerHour();
            Assert.IsTrue(wd.ShouldRunNow(dt));
        }

        [Test]
        public void MultipleTimes10SecondsApart()
        {
            var dt = new DateTime(2017, 1, 6, 0, 0, 0); 
            var wd = GetTriggerSeconds();
            Assert.IsTrue(wd.ShouldRunNow(dt));

            dt = new DateTime(2017, 1, 6, 0, 0, 5);
            Assert.IsTrue(wd.ShouldRunNow(dt));

            dt = new DateTime(2017, 1, 6, 0, 0, 6);
            Assert.IsFalse(wd.ShouldRunNow(dt));


        }

    }
}
