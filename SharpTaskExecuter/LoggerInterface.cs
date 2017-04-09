using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public interface LoggerInterface
    {
        void Debug(string Message);
        void Debug(string Message, params string[] Parameters);
        void Debug(string Message, Exception exception);

        void Info(string Message);
        void Info(string Message, params string[] Parameters);
        void Info(string Message, Exception exception);

        void Warning(string Message);
        void Warning(string Message, params string[] Parameters);
        void Warning(string Message, Exception exception);

        void Error(string Message);
        void Error(string Message, params string[] Parameters);
        void Error(string Message, Exception exception);

        void Fatal(string Message);
        void Fatal(string Message, params string[] Parameters);
        void Fatal(string Message, Exception exception);


    }
}
