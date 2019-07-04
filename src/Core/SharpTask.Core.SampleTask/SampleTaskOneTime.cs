using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.SampleTask
{
    public class SharpTaskSampleTask : ISharpTask
    {
        List<ITriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            var now = DateTime.Now.AddSeconds(5);
            _triggerList = new List<ITriggerInterface>
            {
                new TriggerOneTime(new StDate(now), new StTime(now)) { Name = "+05 sec" }
            };
        }

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                if (_triggerList == null) CreateTriggerList();
                return _triggerList;
            }
        }

        public string Name => "Sample task";

        public string Description => "Sample task description";

        public string Owner => "Sample task owner";

        List<ITriggerInterface> ISharpTask.RunTrigger => throw new NotImplementedException();

        public RunResult RunTask(TaskParameters taskParameters)
        {
            var res = new RunResult(true, true);
            res.LogLines.Add("Things are done");
            return res;
        }
    }
}
