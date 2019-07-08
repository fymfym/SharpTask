using MainTask;
using Xunit;

namespace TaskTest.MainTaskTest
{
    public class CalcTaskTest
    {
        [Fact]
        public void RunTaskTest()
        {
            var task = new CalcTask();
            
            var result = task.RunTask(null);

            Assert.True(result.Successful);
            Assert.True(result.TaskFinished);
        }
    }
}
