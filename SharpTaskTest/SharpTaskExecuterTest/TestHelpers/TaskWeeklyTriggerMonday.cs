using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskWeeklyTriggerMonday : ISharpTask
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

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<ITriggerInterface>();
                var wdl = new List<DayOfWeek>
                {
                    DayOfWeek.Monday
                };
                res.Add(new TriggerWeekday(new STDate(2017, 1, 1), new STTime(12, 00, 00),wdl));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
