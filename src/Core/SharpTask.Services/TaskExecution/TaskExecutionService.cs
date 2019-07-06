
using System;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Repository.TaskExecution;

namespace SharpTask.Core.Services.TaskExecution
{
    public class TaskExecutionService : ITaskExecutionService
    {

        private readonly ITaskExecutionRepository _executionRepository;

        public TaskExecutionService(
            ITaskExecutionRepository executionRepository
            )
        {
            _executionRepository = executionRepository;
        }

        private readonly object _lockObject = new object();
        public void MarkAsStarted(TaskClassState task, DateTime currentTime)
        {
            lock (_lockObject)
            {
                task.LastExecuteStart = currentTime;
                task.ExecutingState = TaskClassState.ExecuteState.Executing;
            }
        }

        public void MarkAsFinishedOk(TaskClassState task, DateTime currentTime)
        {
            lock (_lockObject)
            {
                task.LastExecuteStart = DateTime.MinValue;
                task.LastExecuteFinished = currentTime;
                task.ExecutingState = TaskClassState.ExecuteState.Done;
                task.LatestExecutionResult = ExecutionResult.Ok;
            }
        }

        public void MarkAsFinishedError(TaskClassState task, DateTime currentTime)
        {
            lock (_lockObject)
            {
                task.LastExecuteFinished = currentTime;
                task.LatestExecutionResult = ExecutionResult.Error;
                task.ExecutingState = TaskClassState.ExecuteState.Done;
            }
        }

        public ShouldExecuteResult ShouldExecuteNow(TaskClassState task, DateTime currentTime)
        {

            var result = new ShouldExecuteResult()
            {
                ShouldExecuteNow = false,
                UsedTrigger = null
            };
            var run = true;

            foreach (var tt in task.SharpTask.RunTrigger)
            {
                if (tt.ShouldRunNow(currentTime))
                {
                    if ((task.LastExecuteFinished > DateTime.MinValue) || (task.LastExecuteStart > DateTime.MinValue))
                    {
                        var ts = new TimeSpan(task.LastExecuteFinished.Ticks - currentTime.Ticks).TotalSeconds;
                        if ((ts < 0) && (ts >= -120)) run = false;

                        if (task.LastExecuteStart.Ticks > 0)
                        {
                            ts = new TimeSpan(task.LastExecuteStart.Ticks - currentTime.Ticks).TotalSeconds;
                            if (ts < 0) run = false;
                            
                        }
                    }
                }
                else
                {
                    run = false;
                }

            }

            if (run)
                result.ShouldExecuteNow = true;

            return result;
        }

        public bool ShouldExecuteNow()
        {
            throw new NotImplementedException();
        }
    }
}
