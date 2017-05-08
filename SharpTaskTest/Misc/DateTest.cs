﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharTaskTest.SharpTaskTaskTest
{
[TestFixture]
    public class DateTest
    {
        [Test]
        public void DateCreateTimeDate()
        {
            DateTime dt = new DateTime(2017, 2, 3, 12, 13, 14);

            SharpTask.Date d = new SharpTask.Date(dt);
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

        public void DateCreateTimeInt()
        {

            SharpTask.Date d = new SharpTask.Date(2017,2,3);
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
