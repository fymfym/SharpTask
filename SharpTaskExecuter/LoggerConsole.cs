﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class LoggerConsole : LoggerInterface
    {
        enum LogLevel { Debug, Information, Warning, Error, Fatal }
        void Write(LogLevel Level, string Message)
        {
            Console.WriteLine(string.Format("{0} {1} {2}", DateTime.Now.ToString("HH:mm:ss.fff"), Level, Message));
        }

        public void Debug(string Message)
        {
            Write(LogLevel.Debug, Message);
        }

        public void Debug(string Message, Exception exception)
        {
            Write(LogLevel.Debug, Message);
            Write(LogLevel.Debug, exception.ToString());
        }

        public void Debug(string Message, params string[] Parameters)
        {
            Write(LogLevel.Debug, string.Format(Message,Parameters));
        }

        public void Error(string Message)
        {
            Write(LogLevel.Debug, Message);
        }

        public void Error(string Message, Exception exception)
        {
            Write(LogLevel.Error, Message);
            Write(LogLevel.Error, exception.ToString());
        }

        public void Error(string Message, params string[] Parameters)
        {
            Write(LogLevel.Error, string.Format(Message, Parameters));
        }

        public void Fatal(string Message)
        {
            Write(LogLevel.Fatal, Message);
        }

        public void Fatal(string Message, Exception exception)
        {
            Write(LogLevel.Fatal, Message);
            Write(LogLevel.Fatal, exception.ToString());
        }

        public void Fatal(string Message, params string[] Parameters)
        {
            Write(LogLevel.Fatal, string.Format(Message, Parameters));
        }

        public void Info(string Message)
        {
            Write(LogLevel.Information, Message);
        }

        public void Info(string Message, Exception exception)
        {
            Write(LogLevel.Information, Message);
            Write(LogLevel.Information, exception.ToString());
        }

        public void Info(string Message, params string[] Parameters)
        {
            Write(LogLevel.Information, string.Format(Message, Parameters));
        }

        public void Warning(string Message)
        {
            Write(LogLevel.Warning, Message);
        }

        public void Warning(string Message, Exception exception)
        {
            Write(LogLevel.Warning, Message);
            Write(LogLevel.Warning, exception.ToString());
        }

        public void Warning(string Message, params string[] Parameters)
        {
            Write(LogLevel.Warning, string.Format(Message, Parameters));
        }
    }
}
