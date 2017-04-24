using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SharpTaskExecuter
{
    public class SharpTaskExecuter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharpTaskExecuter));

        Dictionary<Guid, EnquedTask> _enquedTasks;
        SharpTaskExecuterParameter _parameter;
        bool _running;

        public SharpTaskExecuter(SharpTaskExecuterParameter Parameters)
        {
            _parameter = Parameters;
        }

        public void Start()
        {
            log.Info("Starting SharpTaskExecuter");
            _running = true;
            UpdateEnquedTasks();
            ExecuteEnqueuedTasks();
        }

        public void Stop()
        {
            _running = false;
        }
        
        public void ExecuteEnqueuedTasks()
        {
            log.Info("Waiting for enqued tasks start trigger...");
            while (_running)
            {
                foreach(var task in _enquedTasks)
                {
                    var now = DateTime.Now;
                    var res = task.Value.ShouldExecuteNow(now);
                    if (res.ShouldExecuteNow)
                    {
                        log.InfoFormat("Marking task {0} as stared", task.Value.ToString());
                        log.InfoFormat(" - using triger {0}", (res.UsedTrigger.Name != null) ? res.UsedTrigger.Name : "Unnamed");
                        task.Value.MarkAsStarted(now);
                        System.Threading.Thread runTask = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_runTask));
                        runTask.Start(task.Value);
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            log.Info("Enqued tasks stopped!");
        }

        void _runTask(object TaskToRunObject)
        {
            EnquedTask TaskToRun = (EnquedTask)TaskToRunObject;
            log.InfoFormat("Starting task: {0}", TaskToRun.Task.GetType().ToString());

            var result = TaskToRun.Task.RunTask(TaskToRun.Parameters);
            if (result.TaskFinished)
            {
                if (result.Sucessfull)
                {
                    TaskToRun.MarkAsFinishedOk(DateTime.Now);
                    log.InfoFormat("Finished OK: {0}", TaskToRun.Task.GetType().ToString());
                }
                else
                {
                    TaskToRun.MarkAsFinishedError(DateTime.Now);
                    log.ErrorFormat("Finished with error: {0}", TaskToRun.Task.GetType().ToString());
                }
            }
        }

        void UpdateEnquedTasks()
        {
            log.Info("Loading tasks...");
            if (_enquedTasks == null) _enquedTasks = new Dictionary<Guid, EnquedTask>();
            var type = typeof(SharpTaskTask.SharpTaskInterface);

            try
            {
                if (!System.IO.Directory.Exists(_parameter.TaskLibrary)) System.IO.Directory.CreateDirectory(_parameter.TaskLibrary);

                var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _parameter.TaskLibrary);
                if (System.IO.Directory.Exists(path))
                {
                    log.InfoFormat("Loading task .dll files from: {0}", path);
                    var dllFileList = System.IO.Directory.GetFiles(path,"*.dll");
                    foreach (var file in dllFileList)
                    {
                        log.InfoFormat("Investigating file: {0}", System.IO.Path.GetFileName(file));
                        var DLL = Assembly.LoadFile(file);

                        foreach (Type dlltype in DLL.GetExportedTypes())
                        {
                            TypeInfo x = dlltype.GetTypeInfo();
                            var ilist = x.ImplementedInterfaces;

                            if (ilist.Count(z => z.Name.ToLower().Contains("sharptaskinterface")) < 1) continue;
                            var sh = (SharpTaskTask.SharpTaskInterface)Activator.CreateInstance(dlltype);
                            EnquedTask et = new EnquedTask(sh);
                            if (!_enquedTasks.ContainsKey(x.GUID))
                            {
                                _enquedTasks.Add(x.GUID, et);
                                log.InfoFormat("  Task added: {0} / {1}", x.GUID.ToString(), x.Name);
                                foreach (var tt in sh.RunTrigger)
                                {
                                    var name = (tt.Name != null) ? tt.Name : "Unnamed";
                                    var vtd = tt.TriggerDate.ToString();
                                    var vtt = (tt.TriggerTime != null) ? tt.TriggerTime.ToString() : "";
                                    log.InfoFormat("    Trigger: {0} / {1} {2}", name, vtd,  vtt);
                                }
                            }
                        }
                    }
                }
                else
                {
                    log.WarnFormat("Task .dll directory can not be found: {0}", path);
                }
            }
            catch (Exception ex)
            {
                log.Warn("Could not load task .DLL files", ex);
                throw new Exception("Could not load task .DLL files", ex);
            }

        }
    }
}
