using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class EnquedTask
    {
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

        public bool ShouldExecuteNow(DateTime CurrentTime)
        {
            foreach(var tt in _task.RunTrigger)
            {
                if (_lastExecuteFinished > DateTime.MinValue)
                {
                    var ts = new TimeSpan(_lastExecuteFinished.Ticks - CurrentTime.Ticks).TotalSeconds;
                    if (ts <= 0) return false;
                }
                if (tt.ShouldRunNow(CurrentTime))
                {
                    return true;
                }
            }
            return false;
        }
    }
}