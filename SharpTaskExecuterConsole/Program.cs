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

            var Param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);
            new SharpTaskExecuter.SharpTaskExecuter().Start(Param);
        }
    }
}
