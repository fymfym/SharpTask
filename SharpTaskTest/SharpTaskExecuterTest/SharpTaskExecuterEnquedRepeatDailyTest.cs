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
        public void TaskRepeatHourly01Test()
        {
            var t = new TestHelpers.TaskRepeatHourly01();
            var et = new SharpTaskExecuter.EnquedTask(t);

            var dt = new DateTime(2017, 1, 1, 0, 0, 0);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            dt = new DateTime(2017, 1, 1, 0, 1, 0);
            Assert.IsTrue(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsStarted(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            dt = new DateTime(2017, 1, 1, 0, 1, 4);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            et.MarkAsFinishedOk(dt);
            dt = new DateTime(2017, 1, 1, 2, 1, 2);
            Assert.IsTrue(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

    }
}
