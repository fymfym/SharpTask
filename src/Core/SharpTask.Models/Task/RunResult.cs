using System.Collections.Generic;

namespace SharpTask.Core.Models.Task
{
    public class RunResult
    {
        public List<string> LogLines;

        public bool Successful { get; }

        public bool TaskFinished { get; }

        public RunResult(bool taskComplete, bool successful)
        {
            TaskFinished = taskComplete;
            Successful = successful;
            LogLines = new List<string>();
        }
    }
}
