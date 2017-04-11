using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var Param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);

            //var logger = SharpTaskExecuter.SharpTaskExecuter.GetLogger(Param);
            //var logger = new LoggerLog4Net.LoggerLog4Net();

            var logger = new SharpTaskExecuter.LoggerConsole();

            logger.Info("Logger'{0}' instantiated", logger.GetType().ToString());

            var _executer = new SharpTaskExecuter.SharpTaskExecuter(logger);
            _executer.Start(Param);
        }
    }
}
