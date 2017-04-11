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
        List<SharpTaskTask.TaskTriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            var now = DateTime.Now.AddSeconds(5);
            _triggerList = new List<TaskTriggerInterface>();
            _triggerList.Add(new OneTimeTrigger(new Date(now), new Time(now)) { Name = "+05 sec" });
        }

        public List<SharpTaskTask.TaskTriggerInterface> RunTrigger
        {
            get
            {
                if (_triggerList == null) CreateTriggerList();
                return _triggerList;
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
