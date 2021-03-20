using System;
using SharpTask.Task;
using Xunit;

namespace SharTaskTest.SharpTaskTaskTriggerTest
{
    public class TriggerRepeatEveryHourTest
    {
        [Fact]
        public void TestBeforeTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 2, 0);
            var ott = new TriggerRepeatEveryHour(1);
            Assert.False(ott.ShouldRunNow(dt));
        }

        [Fact]
        public void TestAfterTimeOneSecond()
        {
            var dt = new DateTime(2017, 1, 1, 12, 2, 0);
            var ott = new TriggerRepeatEveryHour(2);
            Assert.True(ott.ShouldRunNow(dt.AddSeconds(1)));

            dt = new DateTime(2017, 1, 1, 12, 3, 0);
            ott = new TriggerRepeatEveryHour(2);
            Assert.False(ott.ShouldRunNow(dt.AddSeconds(1)));
        }

        [Fact]
        public void TestOnTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 3, 0);
            var ott = new TriggerRepeatEveryHour(3);
            Assert.True(ott.ShouldRunNow(dt));
        }

        [Fact]
        public void TestAfterTime()
        {
            var dt = new DateTime(2017, 1, 1, 12, 5, 0);
            var ott = new TriggerRepeatEveryHour(4);
            Assert.False(ott.ShouldRunNow(dt));
        }
    }
}
