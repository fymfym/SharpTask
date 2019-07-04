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
