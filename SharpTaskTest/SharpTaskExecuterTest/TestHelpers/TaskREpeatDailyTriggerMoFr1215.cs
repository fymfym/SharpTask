using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskRepeatDailyTriggerMoFr1215 : ISharpTask
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
                var dtr = new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Friday
                };
                res.Add(new TriggerRepeatDaily(new STDate(2017, 1, 1), dtr, new STTime(12, 0, 0)));
                res.Add(new TriggerRepeatDaily(new STDate(2017, 1, 1), dtr, new STTime(15, 0, 0)));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
