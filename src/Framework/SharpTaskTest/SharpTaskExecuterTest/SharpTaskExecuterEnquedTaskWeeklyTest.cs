﻿using System;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskExecuterTest
{
    [TestFixture]
    public class SharpTaskExecuterEnquedTaskOnTime
    {
        
        [Test]
        public void TestEnquedTaskCreation()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.Task == t);
            Assert.IsTrue(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.ExecutionResult.NotSet);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.WaitingForStartTrigger);
        }

        [Test]
        public void TestEnquedMarkAsStarted()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsStarted(DateTime.MinValue);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.Executing);
        }

        [Test]
        public void TestEnquedMarkAsFinishedOk()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsFinishedOk(DateTime.MinValue);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.Done);
            Assert.IsTrue(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.ExecutionResult.Ok);
        }

        [Test]
        public void TestEnquedMarkAsFinishedError()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsFinishedError(DateTime.MinValue);
            Assert.IsTrue(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.Done);
            Assert.IsTrue(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.ExecutionResult.Error);
        }

        [Test]
        public void TestShouldExecuteNowOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt.AddSeconds(1)).ShouldExecuteNow);
        }

        [Test]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Test]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 01);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsTrue(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Test]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 06);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Test]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }


        [Test]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.IsFalse(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}