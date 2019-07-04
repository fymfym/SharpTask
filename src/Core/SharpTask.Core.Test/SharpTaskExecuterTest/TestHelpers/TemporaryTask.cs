using System.Collections.Generic;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Test.SharpTaskExecuterTest.TestHelpers
{
    public class TemporaryTask : ISharpTask
    {
        public string Description => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public string Name => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public string Owner => "SharTaskTest.SharpTaskExecuterTest.TestHelpers";

        public List<ITriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<ITriggerInterface>();
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
