using NUnit.Framework;

namespace SharTaskTest.SharpTaskExecuterTest
{
    [TestFixture]
    public class SharpTaskExecuterFindLoggerTest
    {
        [Test]
        public void NullLogerFetchTest()
        {
            var l = SharpTaskExecuter.SharpTaskExecuter.GetLogger(new SharpTaskExecuter.SharpTaskExecuterParameter());
            Assert.IsTrue(l.GetType().Name == "LoggerConsole");
        }

        [Test]
        public void ConsoleLogerFetchTest()
        {
            var logger = new SharpTaskExecuter.SharpTaskExecuterParameter();
            logger.LoggerClass = "LoggerConsole";
            var l = SharpTaskExecuter.SharpTaskExecuter.GetLogger(logger);
            Assert.IsTrue(l.GetType().Name == "LoggerConsole");
        }

        [Test]
        public void DefaultLogerFetchTest()
        {
            Assert.Throws<System.Exception>(() => SharpTaskExecuter.SharpTaskExecuter.GetLogger(null));
        }

    }
}
