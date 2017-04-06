using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var Param = new SharpTaskExecuter.SharpTaskExecuterParameter();
            Param.TaskLibrary = @"C:\src\SharpTaskExecuter\SharpTaskExecuterConsole\bin\Debug";
            new SharpTaskExecuter.SharpTaskExecuter().Run(Param);
        }
    }
}
