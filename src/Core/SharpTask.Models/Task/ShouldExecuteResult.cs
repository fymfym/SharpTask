namespace SharpTask.Core.Models.Task
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
