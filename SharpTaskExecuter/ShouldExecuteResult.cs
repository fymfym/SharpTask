using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class ShouldExecuteResult
    {
        public bool ShouldExecuteNow;
        public SharpTask.TriggerInterface UsedTrigger;

        public ShouldExecuteResult()
        {
            ShouldExecuteNow = false;
        }
    }
}
