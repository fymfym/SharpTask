using System;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers;
using Xunit;

namespace SharpTask.Core.Test.SharpTaskExecuterTest
{
    public class SharpTaskExecuterDllLoadStateRepeatDailyTest
    {
        [Fact]
        public void TaskRepeatHourly01Test()
        {
            var t = new TaskRepeatHourly01();
            var et = new TaskClassState(null,t);

            //var dt = new DateTime(2017, 1, 1, 0, 0, 0);
            //Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            //dt = new DateTime(2017, 1, 1, 0, 1, 0);
            //Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            //et.MarkAsStarted(dt);
            //Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            //dt = new DateTime(2017, 1, 1, 0, 1, 4);
            //Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            //et.MarkAsFinishedOk(dt);
            //Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            //dt = new DateTime(2017, 1, 1, 2, 1, 2);
            //Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}
