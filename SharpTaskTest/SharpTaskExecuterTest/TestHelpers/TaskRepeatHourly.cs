using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskRepeatHourly01 : ISharpTask
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

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<ITriggerInterface>
                {
                    new TriggerRepeatEveryHour(new STDate(2017, 1, 1), 1)
                };
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
