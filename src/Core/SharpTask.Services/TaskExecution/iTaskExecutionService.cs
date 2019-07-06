
using System;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Services.TaskExecution
{
    public interface ITaskExecutionService
    {
        void MarkAsStarted(TaskClassState task, DateTime currentTime);

        void MarkAsFinishedOk(TaskClassState task, DateTime currentTime);
        void MarkAsFinishedError(TaskClassState task, DateTime currentTime);
        ShouldExecuteResult ShouldExecuteNow(TaskClassState task, DateTime currentTime);

        bool ShouldExecuteNow();
    }
}
