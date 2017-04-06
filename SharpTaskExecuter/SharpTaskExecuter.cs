using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class SharpTaskExecuter
    {

        Dictionary<Guid, EnquedTask> _enquedTasks;
        SharpTaskExecuterParameter _parameter;

        public void Run(SharpTaskExecuterParameter Parameter)
        {
            _parameter = Parameter;
            UpdateEnquedTasks();
            ExecuteEnqueuedTasks();
        }

        void WriteLog(string Message)
        {
            Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss.fff"), Message));
        }

        void WriteLog(string Message, params string[] Messages)
        {
            WriteLog(string.Format(Message, Messages));
        }
        
        public void ExecuteEnqueuedTasks()
        {
            while (true)
            {
                WriteLog("Checking enqued tasks...");
                foreach(var task in _enquedTasks)
                {
                    var now = DateTime.Now;
                    if (task.Value.ShouldExecuteNow(now))
                    {
                        task.Value.MarkAsStarted(now);
                        System.Threading.Thread runTask = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_runTask));
                        runTask.Start(task.Value);
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        void _runTask(object TaskToRunObject)
        {
            EnquedTask TaskToRun = (EnquedTask)TaskToRunObject;
            WriteLog("Starting taks: {0}", TaskToRun.Task.GetType().ToString());

            var result = TaskToRun.Task.RunTask(TaskToRun.Parameters);
            if (result.TaskFinished)
            {
                if (result.Sucessfull)
                {
                    TaskToRun.MarkAsFinishedOk(DateTime.Now);
                    WriteLog("Finished OK: {0}", TaskToRun.Task.GetType().ToString());
                }
                else
                {
                    TaskToRun.MarkAsFinishedError(DateTime.Now);
                    WriteLog("Finished with error: {0}", TaskToRun.Task.GetType().ToString());
                }
            }
        }

        void UpdateEnquedTasks()
        {
            if (_enquedTasks == null) _enquedTasks = new Dictionary<Guid, EnquedTask>();
            var type = typeof(SharpTaskTask.SharpTaskInterface);

            //Loading DLL files and their types
            try
            {
                if (!System.IO.Directory.Exists(_parameter.TaskLibrary)) System.IO.Directory.CreateDirectory(_parameter.TaskLibrary);

                var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _parameter.TaskLibrary);
                if (System.IO.Directory.Exists(path))
                {
                    var dllFileList = System.IO.Directory.GetFiles(path,"*.dll");
                    foreach (var file in dllFileList)
                    {
                        var DLL = Assembly.LoadFile(file);

                        foreach (Type dlltype in DLL.GetExportedTypes())
                        {
                            TypeInfo x = dlltype.GetTypeInfo();
                            var ilist = x.ImplementedInterfaces;

                            if (ilist.Count(z => z.Name.ToLower().Contains("sharptaskinterface")) < 1) continue;
                            var sh = (SharpTaskTask.SharpTaskInterface)Activator.CreateInstance(dlltype);
                            EnquedTask et = new EnquedTask(sh);
                            if (!_enquedTasks.ContainsKey(x.GUID)) _enquedTasks.Add(x.GUID, et);
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not load task .DLL files", ex);
            }

        }
    }
}
