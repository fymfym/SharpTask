using System;
using SharpTask.Task;
using Xunit;

namespace SharTaskTest.Misc
{
    public class TimeTest
    {
        [Fact]
        public void DateCreateTimeDate()
        {
            DateTime dt = new DateTime(2017, 2, 3, 13, 14, 15);

            StTime d = new StTime(dt);
            Assert.True(d.Hour == 13);
            Assert.True(d.Minute == 14);
            Assert.True(d.Second == 15);

            Assert.True(d.DateTimeObject.Year == 1);
            Assert.True(d.DateTimeObject.Month == 1);
            Assert.True(d.DateTimeObject.Day == 1);
            Assert.True(d.DateTimeObject.Hour == 13);
            Assert.True(d.DateTimeObject.Minute == 14);
            Assert.True(d.DateTimeObject.Second == 15);
            Assert.True(d.DateTimeObject.Millisecond == 0);
        }

        [Fact]
        public void DateCreateTimeInt()
        {
            StDate d = new StDate(2017, 2, 3);
            Assert.True(d.Year == 2017);
            Assert.True(d.Month == 2);
            Assert.True(d.Day == 3);

            Assert.True(d.DateTimeObject.Year == 2017);
            Assert.True(d.DateTimeObject.Month == 2);
            Assert.True(d.DateTimeObject.Day == 3);
            Assert.True(d.DateTimeObject.Hour == 0);
            Assert.True(d.DateTimeObject.Minute == 0);
            Assert.True(d.DateTimeObject.Second == 0);
            Assert.True(d.DateTimeObject.Millisecond == 0);
        }
    }
}
