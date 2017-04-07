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

        public static SharpTaskExecuterParameter ParseArgs(string[] args)
        {
            var Param = new SharpTaskExecuterParameter();
            List<string> lst = args.ToList();

            var idx = lst.IndexOf("tasklibrary");
            if (idx < 0)
            {
                Param.TaskLibrary = @"C:\src\SharpTaskExecuter\SharpTaskExecuterConsole\bin\Debug";
            }
            else
            {
                Param.TaskLibrary = args[idx+1];
            }

            return Param;
        }
    }
}
