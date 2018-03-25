using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskTaskTest
{
    [TestFixture]
    public class TimeTest
    {
        [Test]
        public void DateCreateTimeDate()
        {
            DateTime dt = new DateTime(2017, 2, 3, 13, 14, 15);

            STTime d = new STTime(dt);
            Assert.IsTrue(d.Hour == 13);
            Assert.IsTrue(d.Minute == 14);
            Assert.IsTrue(d.Second == 15);

            Assert.IsTrue(d.DateTimeObject.Year == 1);
            Assert.IsTrue(d.DateTimeObject.Month == 1);
            Assert.IsTrue(d.DateTimeObject.Day == 1);
            Assert.IsTrue(d.DateTimeObject.Hour == 13);
            Assert.IsTrue(d.DateTimeObject.Minute == 14);
            Assert.IsTrue(d.DateTimeObject.Second == 15);
            Assert.IsTrue(d.DateTimeObject.Millisecond == 0);

        }

        [Test]
        public void DateCreateTimeInt()
        {

            STDate d = new STDate(2017, 2, 3);
            Assert.IsTrue(d.Year == 2017);
            Assert.IsTrue(d.Month == 2);
            Assert.IsTrue(d.Day == 3);

            Assert.IsTrue(d.DateTimeObject.Year == 2017);
            Assert.IsTrue(d.DateTimeObject.Month == 2);
            Assert.IsTrue(d.DateTimeObject.Day == 3);
            Assert.IsTrue(d.DateTimeObject.Hour == 0);
            Assert.IsTrue(d.DateTimeObject.Minute == 0);
            Assert.IsTrue(d.DateTimeObject.Second == 0);
            Assert.IsTrue(d.DateTimeObject.Millisecond == 0);

        }

    }
}
