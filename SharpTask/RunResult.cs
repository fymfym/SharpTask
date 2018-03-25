using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTask.Task
{
    public class RunResult
    {
        bool _taskFinished;
        bool _sucessfull;
        public List<string> LogLines;

        public bool Sucessfull
        {
            get
            {
                return _sucessfull;
            }
        }

        public bool TaskFinished
        {
            get
            {
                return _taskFinished;
            }
        }

        public RunResult(bool TaskComplete, bool Sucessfull)
        {
            _taskFinished = TaskComplete;
            _sucessfull = Sucessfull;
            LogLines = new List<string>();
        }
    }
}
