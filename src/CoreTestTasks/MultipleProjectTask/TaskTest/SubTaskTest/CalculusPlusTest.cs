using SubTask;
using Xunit;

namespace TaskTest.SubTaskTest
{
    public class CalculusPlusTest
    {
        [Fact]
        public void PlusTest()
        {
            var c = new CalculusPlus();

            var result = c.PlusTwoNumbers(1, 2);

            Assert.Equal(3,result);
        }
        
    }
}
