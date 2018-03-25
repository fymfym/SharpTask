using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharpTaskSampleTask
{
    public class SampleTaskDaily : ISharpTask
    {
        List<ITriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            _triggerList = new List<ITriggerInterface>();
            var stl = new STTime(0,0,0);
            int sec = 30;

            var dl = new List<DayOfWeek>
            {
                DayOfWeek.Thursday
            };

            for (int x = 0; x < 5; x++)
            {
                stl = new STTime(0, 0, sec);
                var rdt = new TriggerRepeatDaily(new STDate(DateTime.Now.AddDays(-1)), dl, stl) { Name = "Every 30 sec trigger - 5 times " };
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

        public string Name
        {
            get
            {
                return "Sample task";
            }
        }

        public string Description
        {
            get
            {
                return "Sample task description";
            }
        }

        public string Owner
        {
            get
            {
                return "Sample task owner";
            }
        }

        List<ITriggerInterface> ISharpTask.RunTrigger => throw new NotImplementedException();

        public RunResult RunTask(TaskParameters taskParameters)
        {
            var res = new RunResult(true, true);
            res.LogLines.Add("Things are done");
            return res;
        }
    }
}
