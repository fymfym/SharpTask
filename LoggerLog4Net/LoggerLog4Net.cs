using System;
using log4net;

namespace LoggerLog4Net
{
    public class LoggerLog4Net : SharpTaskExecuter.LoggerInterface
    {

        public LoggerLog4Net()
        {
            log4net.Config.BasicConfigurator.Configure();
            System.IO.FileInfo configfile = new System.IO.FileInfo("log4net.xml");
            if (configfile.Exists) log4net.Config.XmlConfigurator.Configure(configfile);
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(LoggerLog4Net));

        public void Debug(string Message)
        {
            log.Debug(Message);
        }

        public void Debug(string Message, Exception exception)
        {
            log.Debug(Message,exception);
        }

        public void Debug(string Message, params string[] Parameters)
        {
            log.DebugFormat(Message, Parameters);
        }

        public void Error(string Message)
        {
            log.Error(Message);
        }

        public void Error(string Message, Exception exception)
        {
            log.Error(Message,exception);
        }

        public void Error(string Message, params string[] Parameters)
        {
            log.ErrorFormat(Message, Parameters);

        }

        public void Fatal(string Message)
        {
            log.Fatal(Message);
        }

        public void Fatal(string Message, Exception exception)
        {
            log.Fatal(Message,exception);
        }

        public void Fatal(string Message, params string[] Parameters)
        {
            log.FatalFormat(Message, Parameters);

        }

        public void Info(string Message)
        {
            log.Info(Message);
        }

        public void Info(string Message, Exception exception)
        {
            log.Info(Message,exception);
        }

        public void Info(string Message, params string[] Parameters)
        {
            log.InfoFormat(Message, Parameters);

        }

        public void Warning(string Message)
        {
            log.Warn(Message);
        }

        public void Warning(string Message, Exception exception)
        {
            log.Warn(Message,exception);
        }

        public void Warning(string Message, params string[] Parameters)
        {
            log.WarnFormat(Message, Parameters);
        }
    }
}
