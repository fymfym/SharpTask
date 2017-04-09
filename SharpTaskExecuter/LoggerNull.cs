using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class LoggerNull : LoggerInterface
    {

        public void Debug(string Message)
        {
        }

        public void Debug(string Message, Exception exception)
        {
        }

        public void Debug(string Message, params string[] Parameters)
        {
        }

        public void Error(string Message)
        {
        }

        public void Error(string Message, Exception exception)
        {
        }

        public void Error(string Message, params string[] Parameters)
        {
        }

        public void Fatal(string Message)
        {
        }

        public void Fatal(string Message, Exception exception)
        {
        }

        public void Fatal(string Message, params string[] Parameters)
        {
        }

        public void Info(string Message)
        {
        }

        public void Info(string Message, Exception exception)
        {
        }

        public void Info(string Message, params string[] Parameters)
        {
        }

        public void Warning(string Message)
        {
        }

        public void Warning(string Message, Exception exception)
        {
        }

        public void Warning(string Message, params string[] Parameters)
        {
        }
    }
}
