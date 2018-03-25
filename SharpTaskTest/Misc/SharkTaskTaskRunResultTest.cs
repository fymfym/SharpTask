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
    public class SharkTaskTaskRunResultTest
    {
        [Test]
        public void RunResultFinishedUnsucsecfull()
        {
            var t = new RunResult(true,false);
            Assert.IsTrue(t.TaskFinished);
            Assert.IsFalse(t.Sucessfull);
        }

        [Test]
        public void RunResultUnfinishedSucess()
        {
            var t = new RunResult(false, true);
            Assert.IsFalse(t.TaskFinished);
            Assert.IsTrue(t.Sucessfull);
        }
        [Test]
        public void RunResultUnfinishedUnsucess()
        {
            var t = new RunResult(false, false);
            Assert.IsFalse(t.Sucessfull);
            Assert.IsFalse(t.TaskFinished);
        }
        [Test]
        public void RunResultFinishedSucess()
        {
            var t = new RunResult(true,true);
            Assert.IsTrue(t.Sucessfull);
            Assert.IsTrue(t.TaskFinished);
        }

        [Test]
        public void RunResultLogLines()
        {
            var t = new RunResult(false, false);
            Assert.IsTrue(t.LogLines != null);
        }

    }
}
