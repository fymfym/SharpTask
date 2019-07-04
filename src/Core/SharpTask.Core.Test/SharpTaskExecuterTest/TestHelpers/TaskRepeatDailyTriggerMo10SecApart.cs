using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TaskRepeatDailyTriggerMo10SecApart : ISharpTask
    {
        public string Description => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public string Name => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public string Owner => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<ITriggerInterface>();
                var dtr = new List<DayOfWeek> {DayOfWeek.Sunday};

                res.Add(new TriggerRepeatDaily(new StDate(2017, 1, 1), dtr, new StTime(0,0,0)));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
