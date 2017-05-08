using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SharpTaskExecuter
{
    public class EnquedTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EnquedTask));

        const int StartTimeSlipAllowed = 5;
        object LockObject = new object();

        public SharpTask.TaskParameters Parameters;
        public enum ExecutionResult { NotSet, Ok, Error };
        public enum ExecuteState { WaitingForStartTrigger, InvestigatingTrigger, WaitingToStart, Executing, Done };

        ExecuteState _executionState;
        ExecutionResult _executionResult;
        DateTime _lastExecuteStart;
        DateTime _lastExecuteFinished;
        SharpTask.ISharpTaskInterface _task;
        Dictionary<long,DateTime> _pastExecutions;

        public ExecuteState ExecutingState
        {
            get
            {
                return _executionState;
            }
        }

        public ExecutionResult LatestExecutionResult
        {
            get
            {
                return _executionResult;
            }
        }

        public EnquedTask(SharpTask.ISharpTaskInterface Task)
        {
            if (Task == null) throw new Exception("Task not set correct");
            _task = Task;
            _pastExecutions = new Dictionary<long, DateTime>();
        }

        public SharpTask.ISharpTaskInterface Task
        {
            get
            {
                return _task;
            }
        }

        public void MarkAsStarted(DateTime CurrentTime)
        {
            lock (LockObject)
            {
                _lastExecuteStart = CurrentTime;
                _executionState = ExecuteState.Executing;
            }
        }

        public void MarkAsFinishedOk(DateTime CurrentTime)
        {
            lock (LockObject)
            {
                _lastExecuteStart = DateTime.MinValue;
                _lastExecuteFinished = CurrentTime;
                _executionResult = ExecutionResult.Ok;
                _executionState = ExecuteState.Done;
            }
        }

        public void MarkAsFinishedError(DateTime CurrentTime)
        {
            lock (LockObject)
            {

                _lastExecuteFinished = CurrentTime;
                _executionResult = ExecutionResult.Error;
                _executionState = ExecuteState.Done;
            }
        }

        public ShouldExecuteResult ShouldExecuteNow(DateTime CurrentTime)
        {
            ShouldExecuteResult res = new ShouldExecuteResult();
            foreach (var tt in _task.RunTrigger)
            {
                if (tt.ShouldRunNow(CurrentTime))
                {
                    bool run = true;
                    if ((_lastExecuteFinished > DateTime.MinValue) || (_lastExecuteStart > DateTime.MinValue))
                    {
                        var ts = new TimeSpan(_lastExecuteFinished.Ticks - CurrentTime.Ticks).TotalSeconds;
                        if ((ts <= 0) && (ts >= -120)) run = false;
                        if (_lastExecuteStart.Ticks > 0)
                        {
                            ts = new TimeSpan(_lastExecuteStart.Ticks - CurrentTime.Ticks).TotalSeconds;
                            if (ts <= 0) run = false;
                        }
                    }

                    if (run)
                    {
                        _pastExecutions.Add(tt.GetHashCode(), CurrentTime);
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