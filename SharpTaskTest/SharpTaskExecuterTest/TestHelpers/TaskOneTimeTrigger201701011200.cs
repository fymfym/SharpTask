using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskOneTimeTrigger201701011200 : SharpTaskTask.SharpTaskInterface
    {
        public List<TaskTriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TaskTriggerInterface>();
                res.Add(new SharpTaskTask.OneTimeTrigger(new Date(2017, 1, 1), new Time(12, 00, 00)));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
