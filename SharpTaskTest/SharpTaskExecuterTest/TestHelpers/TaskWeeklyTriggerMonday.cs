using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskWeeklyTriggerMonday : SharpTaskTask.SharpTaskInterface
    {
        public List<TaskTriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TaskTriggerInterface>();
                var wdl = new List<DayOfWeek>();
                wdl.Add(DayOfWeek.Monday);
                res.Add(new SharpTaskTask.WeekdayTrigger(new DateTime(2017, 1, 1, 12, 00, 00),wdl));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
