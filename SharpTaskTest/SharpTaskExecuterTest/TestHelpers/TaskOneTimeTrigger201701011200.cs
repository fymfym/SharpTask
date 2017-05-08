using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskOneTimeTrigger201701011200 : SharpTask.ISharpTaskInterface
    {
        public string Description
        {
            get
            {
                return "SharTaskTest.SharpTaskExecuterTest.TestHelpers";
            }
        }

        public string Name
        {
            get
            {
                return "SharTaskTest.SharpTaskExecuterTest.TestHelpers";
            }
        }

        public string Owner
        {
            get
            {
                return "SharTaskTest.SharpTaskExecuterTest.TestHelpers";
            }
        }

        public List<TriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TriggerInterface>();
                res.Add(new SharpTask.TriggerOneTime(new Date(2017, 1, 1), new Time(12, 00, 00)));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
