using System;
using Xunit;

namespace SharTaskTest.SharpTaskExecuterTest
{
    public class SharpTaskExecuterEnquedRepeatDailyTest
    {
        [Fact]
        public void TaskRepeatHourly01Test()
        {
            var t = new TestHelpers.TaskRepeatHourly01();
            var et = new SharpTaskExecuter.EnquedTask(t);

            var dt = new DateTime(2017, 1, 1, 0, 0, 0);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            dt = new DateTime(2017, 1, 1, 0, 1, 0);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsStarted(dt);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            dt = new DateTime(2017, 1, 1, 0, 1, 4);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);

            dt = new DateTime(2017, 1, 1, 2, 1, 2);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}
