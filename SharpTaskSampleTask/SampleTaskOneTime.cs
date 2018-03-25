using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharpTaskSampleTask
{
    public class SharpTaskSampleTask : ISharpTask
    {
        List<ITriggerInterface> _triggerList;

        private void CreateTriggerList()
        {
            var now = DateTime.Now.AddSeconds(5);
            _triggerList = new List<ITriggerInterface>
            {
                new TriggerOneTime(new STDate(now), new STTime(now)) { Name = "+05 sec" }
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
