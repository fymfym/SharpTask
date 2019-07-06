using System;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers;
using Xunit;

namespace SharpTask.Core.Test.SharpTaskExecuterTest
{
    public class SharpTaskExecuterDllLoadStateTest
    {
        
        [Fact]
        public void TestDllLoadStateTaskCreation()
        {
            var t = new TemporaryTask();
            var et = new TaskClassState(new AssemblyInformation(), t);
//            Assert.True(et.TaskAssembly == t);
            Assert.True(et.LatestExecutionResult == TaskClassState.ExecutionResult.NotSet);
            Assert.True(et.ExecutingState == TaskClassState.ExecuteState.WaitingForStartTrigger);
        }

        [Fact]
        public void TestDllLoadStateMarkAsStarted()
        {
            var t = new TemporaryTask();
            var et = new TaskClassState(null,t);
            et.MarkAsStarted(DateTime.MinValue);
            Assert.True(et.ExecutingState == TaskClassState.ExecuteState.Executing);
        }

        [Fact]
        public void TestDllLoadStateMarkAsFinishedOk()
        {
            var t = new TemporaryTask();
            var et = new TaskClassState(null,t);
            et.MarkAsFinishedOk(DateTime.MinValue);
            Assert.True(et.ExecutingState == TaskClassState.ExecuteState.Done);
            Assert.True(et.LatestExecutionResult == TaskClassState.ExecutionResult.Ok);
        }

        [Fact]
        public void TestDllLoadStateMarkAsFinishedError()
        {
            var t = new TemporaryTask();
            var et = new TaskClassState(null, t);
            et.MarkAsFinishedError(DateTime.MinValue);
            Assert.True(et.ExecutingState == TaskClassState.ExecuteState.Done);
            Assert.True(et.LatestExecutionResult == TaskClassState.ExecutionResult.Error);
        }

        [Fact]
        public void TestShouldExecuteNowOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskClassState(null,t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt.AddSeconds(1)).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskClassState(null,t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 01);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskClassState(null,t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 06);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskClassState(null,t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskClassState(null,t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }


        [Fact]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskClassState(null,t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}
