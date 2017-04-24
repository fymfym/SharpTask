using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SharpTaskExecuterConsole
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            System.IO.FileInfo configfile = new System.IO.FileInfo("log4net");
            if (configfile.Exists) log4net.Config.XmlConfigurator.Configure(configfile);

            log.Info("Console program: On start");

            var Param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);

            var _executer = new SharpTaskExecuter.SharpTaskExecuter(Param);
            _executer.Start();
        }
    }
}
