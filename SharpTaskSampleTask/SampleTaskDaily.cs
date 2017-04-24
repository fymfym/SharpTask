using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharpTaskSampleTask
{
    public class SampleTaskDaily : SharpTaskTask.SharpTaskInterface
    {
        List<SharpTaskTask.TriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            _triggerList = new List<TriggerInterface>();
            var stl = new SharpTaskTask.Time(0,0,0);
            int sec = 30;

            var dl = new List<DayOfWeek>();
            dl.Add(DayOfWeek.Thursday);

            for (int x = 0; x < 5; x++)
            {
                stl = new SharpTaskTask.Time(0, 0, sec);
                var rdt = new SharpTaskTask.TriggerRepeatDaily(new SharpTaskTask.Date(DateTime.Now.AddDays(-1)), dl, stl) { Name = "Every 30 sec trigger - 5 times " };
                _triggerList.Add(rdt);
                sec = sec + 30;
            }

        }

        public List<SharpTaskTask.TriggerInterface> RunTrigger
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

        public RunResult RunTask(TaskParameters taskParameters)
        {
            var res = new RunResult(true, true);
            res.LogLines.Add("Things are done");
            return res;
        }
    }
}
