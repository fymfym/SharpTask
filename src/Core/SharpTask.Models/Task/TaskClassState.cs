using System;
using System.Collections.Generic;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Models.Task
{
    public class TaskClassState
    {
        public DateTime LastExecuteStart;
        public DateTime LastExecuteFinished;

        private ISharpTask _sharpTask;

        public ISharpTask SharpTask => _sharpTask;

        public enum ExecuteState { WaitingForStartTrigger, InvestigatingTrigger, WaitingToStart, Executing, Done };

        public readonly IEnumerable<TaskClassExecution> TaskExecutions;

        public void DisposeClass()
        {
            _sharpTask = null;
        }

        public ExecuteState ExecutingState { get; set; }

        public ExecutionResult LatestExecutionResult { get; set; }

        public TaskClassState(TaskInformation taskInformation, ISharpTask sharpTask)
        {
            _sharpTask = sharpTask ?? throw new Exception("ISharpTask empty/null");
            TaskExecutions = new List<TaskClassExecution>();
        }
    }
}