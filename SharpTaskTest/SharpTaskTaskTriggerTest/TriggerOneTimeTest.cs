using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{
    [TestFixture]
    public class OneTimeTriggerTest
    {

        [Test]
        public void TestBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new SharpTaskTask.TriggerOneTime(new SharpTaskTask.Date(dt), new SharpTaskTask.Time(dt));
            Assert.IsFalse(ott.ShouldRunNow(dt.AddSeconds(-1)));
        }

        [Test]
        public void TestAfterTimeOneSencond()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new SharpTaskTask.TriggerOneTime(new SharpTaskTask.Date(dt), new SharpTaskTask.Time(dt));
            Assert.IsTrue(ott.ShouldRunNow(dt.AddSeconds(1)));
        }

        [Test]
        public void TestOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new SharpTaskTask.TriggerOneTime(new SharpTaskTask.Date(dt), new SharpTaskTask.Time(dt));
            Assert.IsTrue(ott.ShouldRunNow(dt));
        }
    }
}
