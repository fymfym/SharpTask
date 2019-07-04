using System;
using SharpTask.Core.Models.Task;
using Xunit;

namespace SharpTask.Core.Test.SharpTaskTaskTest
{
    public class DateTest
    {
        [Fact]
        public void DateCreateTimeDate()
        {
            DateTime dt = new DateTime(2017, 2, 3, 12, 13, 14);

            StDate d = new StDate(dt);
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

        [Fact]
        public void DateCreateTimeInt()
        {
            var d = new StDate(2017,2,3);
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
