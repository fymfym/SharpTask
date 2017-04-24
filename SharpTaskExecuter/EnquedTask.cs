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

        public SharpTaskTask.TaskParameters Parameters;
        public enum eExecutionResult { NotSet, Ok, Error };
        public enum eExecuteState { WaitingForStartTrigger, InvestigatingTrigger, WaitingToStart, Executing, Done };

        eExecuteState _executionState;
        eExecutionResult _executionResult;
        DateTime _lastExecuteStart;
        DateTime _lastExecuteFinished;
        SharpTaskTask.SharpTaskInterface _task;
        Dictionary<long,DateTime> _pastExecutions;

        public eExecuteState ExecutingState
        {
            get
            {
                return _executionState;
            }
        }

        public eExecutionResult LatestExecutionResult
        {
            get
            {
                return _executionResult;
            }
        }

        public EnquedTask(SharpTaskTask.SharpTaskInterface Task)
        {
            if (Task == null) throw new Exception("Taks not set correct");
            _task = Task;
            _pastExecutions = new Dictionary<long, DateTime>();
        }

        public SharpTaskTask.SharpTaskInterface Task
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
                _executionState = eExecuteState.Executing;
            }
        }

        public void MarkAsFinishedOk(DateTime CurrentTime)
        {
            lock (LockObject)
            {
                _lastExecuteStart = DateTime.MinValue;
                _lastExecuteFinished = CurrentTime;
                _executionResult = eExecutionResult.Ok;
                _executionState = eExecuteState.Done;
            }
        }

        public void MarkAsFinishedError(DateTime CurrentTime)
        {
            lock (LockObject)
            {

                _lastExecuteFinished = CurrentTime;
                _executionResult = eExecutionResult.Error;
                _executionState = eExecuteState.Done;
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
                        if (ts <= 0) run = false;
                        ts = new TimeSpan(_lastExecuteStart.Ticks - CurrentTime.Ticks).TotalSeconds;
                        if (ts <= 0) run = false;
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