﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{

    [TestFixture]
    public class TriggerRepeatDailyTest
    {
 
        ITriggerInterface GetTriggerHour()
        {
            var dtr = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Friday
            };
            return new TriggerRepeatDaily(new StDate(2017, 1, 1), dtr, new StTime(12,0,0));
        }

        ITriggerInterface GetTriggerSeconds()
        {
            var dtr = new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Friday
            };
            return new TriggerRepeatDaily(new StDate(2017, 1, 1), dtr, new StTime(0,0,0) );
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
