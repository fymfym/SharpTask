using System;
using SharpTask.Task;
using Xunit;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{
    public class OneTimeTriggerTest
    {
        [Fact]
        public void TestBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new TriggerOneTime(new StDate(dt), new StTime(dt));
            Assert.False(ott.ShouldRunNow(dt.AddSeconds(-1)));
        }

        [Fact]
        public void TestAfterTimeOneSencond()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new TriggerOneTime(new StDate(dt), new StTime(dt));
            Assert.True(ott.ShouldRunNow(dt.AddSeconds(1)));
        }

        [Fact]
        public void TestOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 0, 0);
            var ott = new TriggerOneTime(new StDate(dt), new StTime(dt));
            Assert.True(ott.ShouldRunNow(dt));
        }
    }
}
