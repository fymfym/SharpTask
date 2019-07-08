using SharpTask.Core.Models.Task;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers;
using Xunit;

namespace SharpTask.Core.Test.ModelsTest
{
    public class TaskClassStateTest
    {

        [Fact]
        public void TestDllLoadStateTaskCreation()
        {
            var t = new TemporaryTask();
            var et = new TaskClassState(new TaskInformation(), t);
            Assert.True(et.LatestExecutionResult == ExecutionResult.NotSet);
            Assert.True(et.ExecutingState == TaskClassState.ExecuteState.WaitingForStartTrigger);
        }

    }
}
