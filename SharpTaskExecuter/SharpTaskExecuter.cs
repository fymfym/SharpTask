using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SharpTask.Task;

namespace SharpTaskExecuter
{
    public class SharpTaskExecuter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharpTaskExecuter));

        Dictionary<Guid, EnquedTask> _enquedTasks;
        SharpTaskExecuterParameter _parameter;
        bool _running;
        DateTime _lastDllLoad;
        Dictionary<string, DLLLoadState> _dllLoadedList;

        public SharpTaskExecuter(SharpTaskExecuterParameter Parameters)
        {
            _parameter = Parameters;
        }

        public void Start()
        {
            log.Info("Starting SharpTaskExecuter");
            _running = true;
            _dllLoadedList = new Dictionary<string, DLLLoadState>();
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
                foreach (var task in _enquedTasks)
                {
                    var now = DateTime.Now;
                    var res = task.Value.ShouldExecuteNow(now);
                    if (res.ShouldExecuteNow)
                    {
                        log.InfoFormat("Marking task {0} as stared", task.Value.ToString());
                        log.InfoFormat(" - using triger {0}", res.UsedTrigger.Name ?? "Unnamed");
                        task.Value.MarkAsStarted(now);
                        System.Threading.Thread runTask = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_runTask));
                        runTask.Start(task.Value);
                    }
                }
                System.Threading.Thread.Sleep(1000);


                if (new TimeSpan(DateTime.Now.Ticks - _lastDllLoad.Ticks).TotalSeconds > 30)
                {
                    UpdateEnquedTasks();
                }
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
            var type = typeof(ISharpTask);

            TypeInfo LoadedTaskType = null;
            DLLLoadState dllLoadState = null;

            foreach (var item in _dllLoadedList)
                item.Value.FilePresenceConfirmed = false;

            try
            {
                if (!System.IO.Directory.Exists(_parameter.TaskLibrary)) System.IO.Directory.CreateDirectory(_parameter.TaskLibrary);

                var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _parameter.TaskLibrary);
                if (System.IO.Directory.Exists(path))
                {
                    log.InfoFormat("Task .dll directoryu: {0}", path);
                    var dllFileList = System.IO.Directory.GetFiles(path, "*.dll");

                    foreach (var file in dllFileList)
                    {
                        log.InfoFormat("Investigating file: {0}", System.IO.Path.GetFileName(file));

                        if (!_dllLoadedList.ContainsKey(file))
                        {
                            dllLoadState = new DLLLoadState()
                            {
                                DLLName = file,
                                DLLFileName = file,
                                FilePresenceConfirmed = true
                            };
                        }
                        else
                        {
                            dllLoadState = _dllLoadedList[file];
                            dllLoadState.FilePresenceConfirmed = true;
                        }

                        var res = _dllLoadedList.Count(x => (x.Value.DLLName == file) && (x.Value.LoadError));
                        if (res > 0) continue;

                        var DLL = Assembly.LoadFile(file);

                        foreach (Type dlltype in DLL.GetExportedTypes())
                        {
                            LoadedTaskType = dlltype.GetTypeInfo();

                            var ilist = LoadedTaskType.ImplementedInterfaces;

                            if (ilist.Count(z => z.Name.ToLower().Contains("sharptaskinterface")) < 1) continue;

                            var sh = (ISharpTask)Activator.CreateInstance(dlltype);

                            EnquedTask et = new EnquedTask(sh);
                            if (!_enquedTasks.ContainsKey(LoadedTaskType.GUID))
                            {
                                _enquedTasks.Add(LoadedTaskType.GUID, et);
                                log.InfoFormat("  Task added: {0} / {1}", LoadedTaskType.GUID.ToString(), LoadedTaskType.GetType().ToString());
                                foreach (var tt in sh.RunTrigger)
                                {
                                    var name = tt.Name ?? "Unnamed";
                                    var vtd = tt.TriggerDate.ToString();
                                    var vtt = (tt.TriggerTime != null) ? tt.TriggerTime.ToString() : "";
                                    log.DebugFormat("    Trigger: {0} / {1} {2}", name, vtd, vtt);
                                    dllLoadState.ConfirmedLoaded = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    log.WarnFormat("Task .dll directory can not be found: {0}", path);
                }

                _lastDllLoad = DateTime.Now;

            }
            catch (Exception ex)
            {
                dllLoadState.LoadError = true;
                if (!_dllLoadedList.ContainsKey(dllLoadState.DLLName))
                {
                    _dllLoadedList.Add(dllLoadState.DLLName, dllLoadState);
                }
                log.WarnFormat("Could not load task .DLL file: {0}", dllLoadState.DLLFileName);
                log.Warn("Loading .DLL failes",ex);
            }

            log.InfoFormat("Task loaded: ");
            foreach (var item in _dllLoadedList.Where(x => !x.Value.ConfirmedLoaded && !x.Value.ReportedToGui))
            {
                log.InfoFormat("  {0}", item.Value.DLLName);
                item.Value.ReportedToGui = true;
            }

            log.InfoFormat("Task unloaded:");
            foreach (var item in _dllLoadedList.Where(x => !x.Value.LoadError && !x.Value.ReportedToGui))
            {
                log.InfoFormat("  {0}", item.Value.DLLName);
                item.Value.ReportedToGui = true;
            }

            log.InfoFormat("DLL files removed:");
            var filelist = _dllLoadedList.Where(x => !x.Value.FilePresenceConfirmed).ToList();
            foreach(var item in filelist)
            {
                _dllLoadedList.Remove(item.Value.DLLName);
                log.InfoFormat("  {0}", item.Value.DLLName);
            }

        }
    }
}
