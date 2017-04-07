using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskExecuterTest
{
    [TestFixture]
    public class SharpTaskExecuterEnquedRepeatDailyTest
    {
        [Test]
        public void TestShouldExecuteTooEraly()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 00);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt));
            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 01);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 06);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 2, 13, 00, 00);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteTwoInARow()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 00);
            var t = new TestHelpers.TaskREpeatDailyTriggerMoFr1215();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt));
            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }
    }
}
