﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTaskTask;

namespace SharTaskTest.SharpTaskExecuterTest.TestHelpers
{
    public class TemporaryTask : SharpTaskTask.SharpTaskInterface
    {
        public string Description
        {
            get
            {
                return "SharTaskTest.SharpTaskExecuterTest.TestHelpers";
            }
        }

        public string Name
        {
            get
            {
                return "SharTaskTest.SharpTaskExecuterTest.TestHelpers";
            }
        }

        public string Owner
        {
            get
            {
                return "SharTaskTest.SharpTaskExecuterTest.TestHelpers";
            }
        }

        public List<TriggerInterface> RunTrigger
        {
            get
            {
                var res = new List<TriggerInterface>();
                return res;
            }
        }

        public RunResult RunTask(TaskParameters taskParameters)
        {
            return new RunResult(true, true);
        }
    }
}
