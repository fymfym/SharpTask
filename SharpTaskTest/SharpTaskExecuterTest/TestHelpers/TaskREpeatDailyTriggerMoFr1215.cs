using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskRepeatDailyTriggerMoFr1215 : SharpTask.ISharpTaskInterface
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
                var dtr = new List<DayOfWeek>();
                dtr.Add(DayOfWeek.Monday);
                dtr.Add(DayOfWeek.Friday);
                res.Add(new SharpTask.TriggerRepeatDaily(new Date(2017, 1, 1), dtr, new Time(12, 0, 0)));
                res.Add(new SharpTask.TriggerRepeatDaily(new Date(2017, 1, 1), dtr, new Time(15, 0, 0)));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
