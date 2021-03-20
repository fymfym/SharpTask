using System;
using Xunit;

namespace SharTaskTest.SharpTaskExecuterTest
{
    public class SharpTaskExecuterEnquedTaskTest
    {
        
        [Fact]
        public void TestEnquedTaskCreation()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.True(et.Task == t);
            Assert.True(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.ExecutionResult.NotSet);
            Assert.True(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.WaitingForStartTrigger);
        }

        [Fact]
        public void TestEnquedMarkAsStarted()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsStarted(DateTime.MinValue);
            Assert.True(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.Executing);
        }

        [Fact]
        public void TestEnquedMarkAsFinishedOk()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsFinishedOk(DateTime.MinValue);
            Assert.True(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.Done);
            Assert.True(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.ExecutionResult.Ok);
        }

        [Fact]
        public void TestEnquedMarkAsFinishedError()
        {
            var t = new TestHelpers.TemporaryTask();
            var et = new SharpTaskExecuter.EnquedTask(t);
            et.MarkAsFinishedError(DateTime.MinValue);
            Assert.True(et.ExecutingState == SharpTaskExecuter.EnquedTask.ExecuteState.Done);
            Assert.True(et.LatestExecutionResult == SharpTaskExecuter.EnquedTask.ExecutionResult.Error);
        }

        [Fact]
        public void TestShouldExecuteNowOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt.AddSeconds(1)).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 01);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 06);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }


        [Fact]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TestHelpers.TaskOneTimeTrigger201701011200();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}
