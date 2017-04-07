using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskREpeatDailyTriggerMoFr1215 : SharpTaskTask.SharpTaskInterface
    {
        public List<TaskTriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TaskTriggerInterface>();
                var dtr = new List<DayOfWeek>();
                dtr.Add(DayOfWeek.Monday);
                dtr.Add(DayOfWeek.Friday);
                var ttr = new List<Time>();
                ttr.Add(new Time(12, 0, 0));
                ttr.Add(new Time(15, 0, 0));
                res.Add(new SharpTaskTask.RepeatDailyTrigger(new Date(2017, 1, 1), dtr, ttr));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
