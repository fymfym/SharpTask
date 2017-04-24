using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharpTaskSampleTask
{
    public class SharpTaskSampleTask : SharpTaskTask.SharpTaskInterface
    {
        List<SharpTaskTask.TriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            var now = DateTime.Now.AddSeconds(5);
            _triggerList = new List<TriggerInterface>();
            _triggerList.Add(new TriggerOneTime(new Date(now), new Time(now)) { Name = "+05 sec" });
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
