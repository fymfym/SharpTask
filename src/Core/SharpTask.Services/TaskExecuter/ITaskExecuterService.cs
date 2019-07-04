using System;
using System.Collections.Generic;
using System.Text;

namespace SharpTask.Core.Services.TaskExecuter
{
    public interface ITaskExecuterService
    {
        void Start();
        void Stop();
    }
}
