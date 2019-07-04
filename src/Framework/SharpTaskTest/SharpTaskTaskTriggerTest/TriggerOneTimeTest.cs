using System;
using NUnit.Framework;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{
    [TestFixture]
    public class OneTimeTriggerTest
    {

        [Test]
        public void TestBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new TriggerOneTime(new StDate(dt), new StTime(dt));
            Assert.IsFalse(ott.ShouldRunNow(dt.AddSeconds(-1)));
        }

        [Test]
        public void TestAfterTimeOneSencond()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new TriggerOneTime(new StDate(dt), new StTime(dt));
            Assert.IsTrue(ott.ShouldRunNow(dt.AddSeconds(1)));
        }

        [Test]
        public void TestOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new TriggerOneTime(new StDate(dt), new StTime(dt));
            Assert.IsTrue(ott.ShouldRunNow(dt));
        }
    }
}
