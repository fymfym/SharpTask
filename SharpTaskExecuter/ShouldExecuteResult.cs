using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTask.Task;

namespace SharpTaskExecuter
{
    public class ShouldExecuteResult
    {
        public bool ShouldExecuteNow;
        public ITriggerInterface UsedTrigger;

        public ShouldExecuteResult()
        {
            ShouldExecuteNow = false;
        }
    }
}
