using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskExecuterTest
{
    [TestFixture]
    public class SharpTaskExecuterEnquedTask
    {
        
        [Test]
        public void TestEnquedTaskCreation()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.Task == t);
            Assert.IsTrue(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.eExecutionResult.NotSet);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.eExecuteState.WaitingForStartTrigger);
        }

        [Test]
        public void TestEnquedMarkAsStarted()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsStarted(DateTime.MinValue);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.eExecuteState.Executing);
        }

        [Test]
        public void TestEnquedMarkAsFinishedOk()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsFinishedOk(DateTime.MinValue);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.eExecuteState.Done);
            Assert.IsTrue(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.eExecutionResult.Ok);
        }

        [Test]
        public void TestEnquedMarkAsFinishedError()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsFinishedError(DateTime.MinValue);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.eExecuteState.Done);
            Assert.IsTrue(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.eExecutionResult.Error);
        }

        [Test]
        public void TestShouldExecuteNowOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt));
            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt.AddSeconds(1)));
        }

        [Test]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt));
            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 01);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 06);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }

        [Test]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }


        [Test]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt));
        }
    }
}
