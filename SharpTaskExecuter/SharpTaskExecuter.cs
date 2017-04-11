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
        LoggerInterface _logger;
        bool _running;

        public static LoggerInterface GetLogger(SharpTaskExecuterParameter Parameters)
        {
            if (Parameters == null) throw new Exception("Parameters can not be null");

            Assembly assembly = Assembly.Load(Parameters.LoggerAssembly);
            if (assembly == null) throw new Exception("Assembly name in 'LoggerAssembly' not valid");

            var x = assembly.GetTypes().Count(t => t.Name.ToLower() == Parameters.LoggerClass.ToLower());
            if (x == 0) throw new Exception("Assembly name in 'LoggerClass' not valid");

            var loggerType = assembly.GetTypes().First(t => t.Name.ToLower() == Parameters.LoggerClass.ToLower());

            return (LoggerInterface)Activator.CreateInstance(loggerType);

        }

        public SharpTaskExecuter(LoggerInterface Logger)
        {
            if (Logger == null) throw new Exception("Logger must be implemented");
            _logger = Logger;
        }

        public void Start(SharpTaskExecuterParameter Parameter)
        {
            _logger.Info("Starting SharpTaskExecuter");
            _parameter = Parameter;
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
            _logger.Info("Enqued tasks starting...");
            while (_running)
            {
                foreach(var task in _enquedTasks)
                {
                    var now = DateTime.Now;
                    if (task.Value.ShouldExecuteNow(now))
                    {
                        _logger.Info("Marking task {0} as stared", task.Value.ToString());
                        task.Value.MarkAsStarted(now);
                        System.Threading.Thread runTask = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_runTask));
                        runTask.Start(task.Value);
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            _logger.Info("Enqued tasks stopped!");
        }

        void _runTask(object TaskToRunObject)
        {
            EnquedTask TaskToRun = (EnquedTask)TaskToRunObject;
            _logger.Info("Starting taks: {0}", TaskToRun.Task.GetType().ToString());

            var result = TaskToRun.Task.RunTask(TaskToRun.Parameters);
            if (result.TaskFinished)
            {
                if (result.Sucessfull)
                {
                    TaskToRun.MarkAsFinishedOk(DateTime.Now);
                    _logger.Info("Finished OK: {0}", TaskToRun.Task.GetType().ToString());
                }
                else
                {
                    TaskToRun.MarkAsFinishedError(DateTime.Now);
                    _logger.Error("Finished with error: {0}", TaskToRun.Task.GetType().ToString());
                }
            }
        }

        void UpdateEnquedTasks()
        {
            _logger.Info("Loading tasks...");
            if (_enquedTasks == null) _enquedTasks = new Dictionary<Guid, EnquedTask>();
            var type = typeof(SharpTaskTask.SharpTaskInterface);

            try
            {
                if (!System.IO.Directory.Exists(_parameter.TaskLibrary)) System.IO.Directory.CreateDirectory(_parameter.TaskLibrary);

                var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _parameter.TaskLibrary);
                if (System.IO.Directory.Exists(path))
                {
                    _logger.Info("Loading task .dll files from: {0}", path);
                    var dllFileList = System.IO.Directory.GetFiles(path,"*.dll");
                    foreach (var file in dllFileList)
                    {
                        _logger.Info("Investigating file: {0}", System.IO.Path.GetFileName(file));
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
                                _logger.Info("  Task added: {0} / {1}", x.GUID.ToString(), x.Name);
                                foreach (var tt in sh.RunTrigger)
                                {
                                    _logger.Info("    Trigger: {0} / {1} {2}", tt.Name, tt.TriggerDate.ToString(), tt.TriggerTime.ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    _logger.Warning("Task .dll directory can not be found: {0}", path);
                }
            }
            catch (Exception ex)
            {
                _logger.Warning("Could not load task .DLL files", ex);
                throw new Exception("Could not load task .DLL files", ex);
            }

        }
    }
}
