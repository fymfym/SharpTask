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
        public List<SharpTaskTask.TaskTriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<SharpTaskTask.TaskTriggerInterface>();
                res.Add(new OneTimeTrigger(DateTime.Now.AddSeconds(1)) { Name = "+01 sec" });
                res.Add(new OneTimeTrigger(DateTime.Now.AddSeconds(10)) { Name = "+10 sec" });
                return res;
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
