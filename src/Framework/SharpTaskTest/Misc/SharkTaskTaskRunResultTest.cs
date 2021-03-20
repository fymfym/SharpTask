using SharpTask.Task;
using Xunit;

namespace SharTaskTest.Misc
{
    public class SharkTaskTaskRunResultTest
    {
        [Fact]
        public void RunResultFinishedUnsucsecfull()
        {
            var t = new RunResult(true,false);
            Assert.True(t.TaskFinished);
            Assert.False(t.Sucessfull);
        }

        [Fact]
        public void RunResultUnfinishedSucess()
        {
            var t = new RunResult(false, true);
            Assert.False(t.TaskFinished);
            Assert.True(t.Sucessfull);
        }

        [Fact]
        public void RunResultUnfinishedUnsucess()
        {
            var t = new RunResult(false, false);
            Assert.False(t.Sucessfull);
            Assert.False(t.TaskFinished);
        }

        [Fact]
        public void RunResultFinishedSuccess()
        {
            var t = new RunResult(true,true);
            Assert.True(t.Sucessfull);
            Assert.True(t.TaskFinished);
        }

        [Fact]
        public void RunResultLogLines()
        {
            var t = new RunResult(false, false);
            Assert.True(t.LogLines != null);
        }
    }
}
