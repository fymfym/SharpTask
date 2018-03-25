using System;
using System.Collections.Generic;
using SharpTask.Task;

namespace SharpTaskSampleTask
{
    public class SampleTaskDaily : ISharpTask
    {
        List<ITriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            _triggerList = new List<ITriggerInterface>();
            int sec = 30;

            var dl = new List<DayOfWeek>
            {
                DayOfWeek.Thursday
            };

            for (int x = 0; x < 5; x++)
            {
                var stl = new StTime(0, 0, sec);
                var rdt = new TriggerRepeatDaily(new StDate(DateTime.Now.AddDays(-1)), dl, stl) { Name = "Every 30 sec trigger - 5 times " };
                _triggerList.Add(rdt);
                sec = sec + 30;
            }

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
