using System;
using Xunit;

namespace SharTaskTest.SharpTaskExecuterTest
{
    public class SharpTaskExecuterEnquedTaskWeekly
    {
        [Fact]
        public void TestShouldExecuteTooEraly()
        {
            var dt = new DateTime(2017, 1, 1, 12, 00, 00);
            var t = new TestHelpers.TaskWeeklyTriggerMonday();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteNowTwoTimesOnTime()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 00);
            var t = new TestHelpers.TaskWeeklyTriggerMonday();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
            et.MarkAsFinishedOk(dt);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirstTimeOneSecondOver()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 01);
            var t = new TestHelpers.TaskWeeklyTriggerMonday();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.True(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteFirsTimeSixSecondOver()
        {
            var dt = new DateTime(2017, 1, 2, 12, 00, 06);
            var t = new TestHelpers.TaskWeeklyTriggerMonday();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }

        [Fact]
        public void TestShouldExecuteBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 11, 00, 00);
            var t = new TestHelpers.TaskWeeklyTriggerMonday();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }


        [Fact]
        public void TestShouldExecuteAfter1HTime()
        {
            var dt = new DateTime(2017, 1, 1, 13, 00, 00);
            var t = new TestHelpers.TaskWeeklyTriggerMonday();
            var et = new SharpTaskExecuter.EnquedTask(t);
            Assert.False(et.ShouldExecuteNow(dt).ShouldExecuteNow);
        }
    }
}
