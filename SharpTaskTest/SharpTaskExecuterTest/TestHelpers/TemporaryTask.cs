using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TemporaryTask : SharpTaskTask.SharpTaskInterface
    {
        public List<TaskTriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TaskTriggerInterface>();
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
