using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.SampleTask
{
    public class SharpTaskSampleTask : ISharpTask
    {

        private List<ITriggerInterface> CreateTriggerList()
        {
            var now = DateTime.Now.AddSeconds(5);
            return new List<ITriggerInterface>
            {
                new TriggerOneTime(new StDate(now), new StTime(now)) { Name = "Now +05 sec" }
            };
        }


        public string Name => "Sample task";

        public string Description => "Sample task description";

        public string Owner => "Sample task owner";

        List<ITriggerInterface> ISharpTask.RunTrigger => CreateTriggerList();

        public RunResult RunTask(TaskParameters taskParameters)
        {
            var res = new RunResult(true, true);
            res.LogLines.Add("Things are done");
            return res;
        }
    }
}
