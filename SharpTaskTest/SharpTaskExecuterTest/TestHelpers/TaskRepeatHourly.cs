using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskRepeatHourly01 : SharpTaskTask.SharpTaskInterface
    {
        public string Description
        {
            get
            {
                return "TaskRepeatHourly";
            }
        }

        public string Name
        {
            get
            {
                return "TaskRepeatHourly";
            }
        }

        public string Owner
        {
            get
            {
                return "TaskRepeatHourly";
            }
        }

        public List<TriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TriggerInterface>();
                res.Add(new SharpTaskTask.TriggerRepeatEveryHour(new Date(2017, 1, 1), 1));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
