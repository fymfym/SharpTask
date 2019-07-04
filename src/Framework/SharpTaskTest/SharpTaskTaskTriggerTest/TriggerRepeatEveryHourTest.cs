using System;
using NUnit.Framework;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{
    [TestFixture]
    public class TriggerRepeatEveryHourTest
    {

        [Test]
        public void TestBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 2, 0);
            var ott = new TriggerRepeatEveryHour(1);
            Assert.IsFalse(ott.ShouldRunNow(dt));
        }

        [Test]
        public void TestAfterTimeOneSecond()
        {
            var dt = new DateTime(2017, 1, 1, 12, 2, 0);
            var ott = new TriggerRepeatEveryHour(2);
            Assert.IsTrue(ott.ShouldRunNow(dt.AddSeconds(1)));

            dt = new DateTime(2017, 1, 1, 12, 3, 0);
            ott = new TriggerRepeatEveryHour(2);
            Assert.IsFalse(ott.ShouldRunNow(dt.AddSeconds(1)));

        }

        [Test]
        public void TestOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 3, 0);
            var ott = new TriggerRepeatEveryHour(3);
            Assert.IsTrue(ott.ShouldRunNow(dt));
        }

        [Test]
        public void TestAfterTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 5, 0);
            var ott = new TriggerRepeatEveryHour(4);
            Assert.IsFalse(ott.ShouldRunNow(dt));
        }

    }
}
