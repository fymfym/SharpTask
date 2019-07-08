using System;
using System.Collections.Generic;
using SharpTask.Core.Models.Schedule;
using SharpTask.Core.Models.Task;
using SubTask;

namespace MainTask
{
    public class CalcTask: ISharpTask
    {
        public RunResult RunTask(TaskParameters taskParameters)
        {
            var calcClass = new CalculusPlus();

            return new RunResult(true,true)
            {


                LogLines = new List<string>()
                {
                    $"Result = {calcClass.PlusTwoNumbers(1,2)}"
                }
            };
        }

        public List<ITriggerInterface> RunTrigger => new List<ITriggerInterface>()
        {
            new TriggerOneTime(new StDate(DateTime.Now), new StTime(DateTime.Now.AddSeconds(30)) )
        };
        public string Name => "Calculus task"; 
        public string Description => "Calculate things";
        public string Owner => "FYM";
    }
}
