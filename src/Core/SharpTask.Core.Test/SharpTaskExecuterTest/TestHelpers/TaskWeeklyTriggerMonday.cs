using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers
{
    public class TaskWeeklyTriggerMonday : ISharpTask
    {
        public string Description => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public string Name => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public string Owner => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<ITriggerInterface>();
                var wdl = new List<DayOfWeek>
                {
                    DayOfWeek.Monday
                };
                res.Add(new TriggerWeekday(new StDate(2017, 1, 1), new StTime(12, 00, 00),wdl));
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
