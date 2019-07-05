using System;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers;
using Xunit;

namespace SharpTask.Core.Test.SharpTaskExecuterTest
{
    public class SharpTaskExecuterDllLoadStateOnTime
    {
        
        [Fact]
        public void TestDllLoadStateCreation()
        {
            var t = new TemporaryTask();
            var et = new DllLoadState(null,t);
            Assert.True(et.TaskInstance == t);
            Assert.True(et.LatestExecutionResult == DllLoadState.ExecutionResult.NotSet);
            Assert.True(et.ExecutingState == DllLoadState.ExecuteState.WaitingForStartTrigger);
        }

        [Fact]
        public void TestDllLoadStateMarkAsStarted()
        {
            var t = new TemporaryTask();
            var et = new DllLoadState(null,t);
            et.MarkAsStarted(DateTime.MinValue);
            Assert.True(et.ExecutingState == DllLoadState.ExecuteState.Executing);
        }

        [Fact]
        public void TestDllLoadStateMarkAsFinishedOk()
        {
            var t = new TemporaryTask();
            var et = new DllLoadState(null,t);
            et.MarkAsFinishedOk(DateTime.MinValue);
            Assert.True(et.ExecutingState == DllLoadState.ExecuteState.Done);
            Assert.True(et.LatestExecutionResult == DllLoadState.ExecutionResult.Ok);
        }

        [Fact]
        public void TestDllLoadStateMarkAsFinishedError()
        {
            var t = new TemporaryTask();
            var et = new DllLoadState(null,t);
            et.MarkAsFinishedError(DateTime.MinValue);
            Assert.True(et.ExecutingState == DllLoadState.ExecuteState.Done);
            Assert.True(et.LatestExecutionResult == DllLoadState.ExecutionResult.Error);
        }

        [Fact]
        public void TestShouldExecuteNowOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new DllLoadState(null,t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt.AddSeconds(1)).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new DllLoadState(null,t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 01);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new DllLoadState(null,t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 06);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new DllLoadState(null,t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new DllLoadState(null,t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }


        [Fact]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new DllLoadState(null,t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}
