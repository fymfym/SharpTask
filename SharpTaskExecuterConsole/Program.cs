using log4net;

namespace SharpTaskExecuterConsole
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            System.IO.FileInfo configfile = new System.IO.FileInfo("log4net");
            if (configfile.Exists) log4net.Config.XmlConfigurator.Configure(configfile);

            Log.Info("Console program: On start");

            var param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);

            var executer = new SharpTaskExecuter.SharpTaskExecuter(param);
            executer.Start();
        }
    }
}
