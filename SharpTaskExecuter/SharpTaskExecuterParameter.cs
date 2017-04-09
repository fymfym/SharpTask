using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class SharpTaskExecuterParameter
    {
        public string TaskLibrary;
        public string LoggerAssembly;
        public string LoggerClass;

        public SharpTaskExecuterParameter()
        {
            LoggerAssembly = "SharpTaskExecuter";
            LoggerClass = "LoggerConsole";
            TaskLibrary = ".";
        }

        static string GetParameterValue(List<string> KeyValueList, string Name, string DefaultVaue)
        {
            var idx = KeyValueList.IndexOf(Name);
            if (idx >= 0)
                if (KeyValueList.Count() >= idx + 1)
                    return KeyValueList[idx + 1];
            return DefaultVaue;
        }

        public static SharpTaskExecuterParameter ParseArgs(string[] args)
        {
            var Param = new SharpTaskExecuterParameter();
            List<string> lst = args.ToList();

            Param.TaskLibrary = GetParameterValue(lst, "tasklibrary", @"C:\src\SharpTaskExecuter\SharpTaskExecuterConsole\bin\Debug");
            Param.LoggerAssembly = GetParameterValue(lst, "loggerassembly", "SharpTaskExecuter");
            Param.LoggerClass = GetParameterValue(lst, "loggerclass", "LoggerConsole");
            

            return Param;
        }
    }
}
