using System.Collections.Generic;

namespace SharpTask.Task
{
    public class RunResult
    {
        public List<string> LogLines;

        public bool Sucessfull { get; }

        public bool TaskFinished { get; }

        public RunResult(bool taskComplete, bool sucessfull)
        {
            TaskFinished = taskComplete;
            Sucessfull = sucessfull;
            LogLines = new List<string>();
        }
    }
}
