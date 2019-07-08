using System;
using FakeItEasy;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Repository.TaskExecution;
using SharpTask.Core.Services.TaskExecution;
using SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers;
using Xunit;

namespace SharpTask.Core.Test.ServicesTest
{
    public class TaskExecutionServiceTest
    {

        private readonly ITaskExecutionRepository _fakedRepository;
        private readonly TaskClassState _task;

        public TaskExecutionServiceTest()
        {
            _fakedRepository = A.Fake<ITaskExecutionRepository>();
            _task = new TaskClassState(new TaskInformation(), new TemporaryTask());
        }

        [Fact]
        public void TaskExecutionServiceMarkAsStarted()
        {
            var et = new TaskExecutionService(_fakedRepository);
            
            et.MarkAsStarted(_task, DateTime.MinValue);
            
            Assert.True(TaskClassState.ExecuteState.Executing == _task.ExecutingState);
        }

        [Fact]
        public void TestDllLoadStateMarkAsFinishedOk()
        {
            var et = new TaskExecutionService(_fakedRepository);

            et.MarkAsFinishedOk(_task, DateTime.MinValue);
            
            Assert.True(_task.ExecutingState == TaskClassState.ExecuteState.Done);
            Assert.True(_task.LatestExecutionResult == ExecutionResult.Ok);
        }

        [Fact]
        public void TestDllLoadStateMarkAsFinishedError()
        {
            var et = new TaskExecutionService(_fakedRepository);
            
            et.MarkAsFinishedError(_task, DateTime.MinValue);
            
            Assert.True(_task.ExecutingState == TaskClassState.ExecuteState.Done);
            Assert.True(_task.LatestExecutionResult == ExecutionResult.Error);
        }

        [Fact]
        public void TestShouldExecuteNowOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskExecutionService(_fakedRepository);
            var task = new TaskClassState(new TaskInformation(), t);
            
            et.MarkAsFinishedOk(task,dt);

            Assert.True(et.ShouldExecuteNow(task, dt).ShouldExecuteNow);
            Assert.False(et.ShouldExecuteNow(task, dt.AddSeconds(1)).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var et = new TaskExecutionService(_fakedRepository);
            var task = new TaskClassState(new TaskInformation(), t);
            
            et.MarkAsFinishedOk(task, dt);

            Assert.True(et.ShouldExecuteNow(task, dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 01);
            var t = new TaskOneTimeTrigger201701011200();
            var task = new TaskClassState(new TaskInformation(), t);
            var et = new TaskExecutionService(_fakedRepository);
         
            Assert.True(et.ShouldExecuteNow(task, dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 06);
            var t = new TaskOneTimeTrigger201701011200();
            var task = new TaskClassState(new TaskInformation(), t);
            var et = new TaskExecutionService(_fakedRepository);

            Assert.False(et.ShouldExecuteNow(task,dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var task = new TaskClassState(new TaskInformation(), t);
            var et = new TaskExecutionService(_fakedRepository);

            Assert.False(et.ShouldExecuteNow(task, dt).ShouldExecuteNow);
        }


        [Fact]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TaskOneTimeTrigger201701011200();
            var task = new TaskClassState(new TaskInformation(), t);
            var et = new TaskExecutionService(_fakedRepository);
            Assert.False(et.ShouldExecuteNow(task,dt).ShouldExecuteNow);
        }

    }
}
