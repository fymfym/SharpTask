using System;
using System.Collections.Generic;
using SharpTask.Core.Models.TaskModule;

namespace SharpTask.Core.Models.Task
{
    public class TaskClassState
    {
        // private static readonly ILog Log = LogManager.GetLogger(typeof(TaskClassState));

        private readonly object _lockObject = new object();
        private DateTime _lastExecuteStart;
        private DateTime _lastExecuteFinished;

        public AssemblyInformation AssemblyInformation { get; set; }
        private ISharpTask _sharpTask;

        public long DllHash;

        public AssemblyInformation TaskInformation;

        public TaskParameters Parameters;
        public enum ExecutionResult { NotSet, Ok, Error };
        public enum ExecuteState { WaitingForStartTrigger, InvestigatingTrigger, WaitingToStart, Executing, Done };

        public readonly Dictionary<long,DateTime> PastExecutions;

        public void DisposeClass()
        {
            _sharpTask = null;
        }

        public ExecuteState ExecutingState { get; private set; }

        public ExecutionResult LatestExecutionResult { get; private set; }

        public TaskClassState(AssemblyInformation taskInformation, ISharpTask sharpTask)
        {
            _sharpTask = sharpTask ?? throw new Exception("ISharpTask empty/null");
            PastExecutions = new Dictionary<long, DateTime>();
        }

        public void MarkAsStarted(DateTime currentTime)
        {
            lock (_lockObject)
            {
                _lastExecuteStart = currentTime;
                ExecutingState = ExecuteState.Executing;
            }
        }

        public void MarkAsFinishedOk(DateTime currentTime)
        {
            lock (_lockObject)
            {
                _lastExecuteStart = DateTime.MinValue;
                _lastExecuteFinished = currentTime;
                LatestExecutionResult = ExecutionResult.Ok;
                ExecutingState = ExecuteState.Done;
            }
        }

        public void MarkAsFinishedError(DateTime currentTime)
        {
            lock (_lockObject)
            {

                _lastExecuteFinished = currentTime;
                LatestExecutionResult = ExecutionResult.Error;
                ExecutingState = ExecuteState.Done;
            }
        }

        public ShouldExecuteResult ShouldExecuteNow(DateTime currentTime)
        {
            ShouldExecuteResult res = new ShouldExecuteResult();
            foreach (var tt in _sharpTask.RunTrigger)
            {
                if (tt.ShouldRunNow(currentTime))
                {
                    bool run = true;
                    if ((_lastExecuteFinished > DateTime.MinValue) || (_lastExecuteStart > DateTime.MinValue))
                    {
                        var ts = new TimeSpan(_lastExecuteFinished.Ticks - currentTime.Ticks).TotalSeconds;
                        if ((ts <= 0) && (ts >= -120)) run = false;
                        if (_lastExecuteStart.Ticks > 0)
                        {
                            ts = new TimeSpan(_lastExecuteStart.Ticks - currentTime.Ticks).TotalSeconds;
                            if (ts <= 0) run = false;
                        }
                    }

                    if (run)
                    {
                        PastExecutions.Add(tt.GetHashCode(), currentTime);
                        res.ShouldExecuteNow = true;
                        res.UsedTrigger = tt;
                        return res;
                    }
                }
            }
            return res;
        }
    }
}