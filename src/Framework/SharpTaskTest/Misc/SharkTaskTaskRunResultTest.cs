using Xunit;
using SharpTask.Task;

namespace SharTaskTest.Misc
{
    public class SharkTaskTaskRunResultTest
    {
        [Fact]
        public void RunResultFinishedUnsucsecfull()
        {
            var t = new RunResult(true,false);
            Assert.IsTrue(t.TaskFinished);
            Assert.IsFalse(t.Sucessfull);
        }

        [Fact]
        public void RunResultUnfinishedSucess()
        {
            var t = new RunResult(false, true);
            Assert.IsFalse(t.TaskFinished);
            Assert.IsTrue(t.Sucessfull);
        }
        [Fact]
        public void RunResultUnfinishedUnsucess()
        {
            var t = new RunResult(false, false);
            Assert.IsFalse(t.Sucessfull);
            Assert.IsFalse(t.TaskFinished);
        }
        [Fact]
        public void RunResultFinishedSucess()
        {
            var t = new RunResult(true,true);
            Assert.IsTrue(t.Sucessfull);
            Assert.IsTrue(t.TaskFinished);
        }

        [Fact]
        public void RunResultLogLines()
        {
            var t = new RunResult(false, false);
            Assert.IsTrue(t.LogLines != null);
        }

    }
}
