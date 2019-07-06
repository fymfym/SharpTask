using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers
{
    public class TaskRepeatHourly01 : ISharpTask
    {
        public string Description => "TaskRepeatHourly";

        public string Name => "TaskRepeatHourly";

        public string Owner => "TaskRepeatHourly";

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<ITriggerInterface>
                {
                    new TriggerRepeatEveryHour(1)
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
