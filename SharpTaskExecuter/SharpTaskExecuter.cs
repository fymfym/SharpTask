using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using SharpTask.Task;

namespace SharpTaskExecuter
{
    public class SharpTaskExecuter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SharpTaskExecuter));

        Dictionary<Guid, EnquedTask> _enquedTasks;
        readonly SharpTaskExecuterParameter _parameter;
        bool _running;
        DateTime _lastDllLoad;
        Dictionary<string, DllLoadState> _dllLoadedList;

        public SharpTaskExecuter(SharpTaskExecuterParameter parameters)
        {
            _parameter = parameters;
        }

        public void Start()
        {
            Log.Info("Starting SharpTaskExecuter");
            _running = true;
            _dllLoadedList = new Dictionary<string, DllLoadState>();
            UpdateEnquedTasks();
            ExecuteEnqueuedTasks();
        }

        public void Stop()
        {
            _running = false;
        }

        public void ExecuteEnqueuedTasks()
        {
            Log.Info("Waiting for enqued tasks start trigger...");
            while (_running)
            {
                foreach (var task in _enquedTasks)
                {
                    var now = DateTime.Now;
                    var res = task.Value.ShouldExecuteNow(now);
                    if (res.ShouldExecuteNow)
                    {
                        Log.InfoFormat("Marking task {0} as stared", task.Value);
                        Log.InfoFormat(" - using triger {0}", res.UsedTrigger.Name ?? "Unnamed");
                        task.Value.MarkAsStarted(now);
                        System.Threading.Thread runTask = new System.Threading.Thread(_runTask);
                        runTask.Start(task.Value);
                    }
                }
                System.Threading.Thread.Sleep(1000);


                if (new TimeSpan(DateTime.Now.Ticks - _lastDllLoad.Ticks).TotalSeconds > 30)
                {
                    UpdateEnquedTasks();
                }
            }
            Log.Info("Enqued tasks stopped!");
        }

        void _runTask(object taskToRunObject)
        {
            EnquedTask taskToRun = (EnquedTask)taskToRunObject;
            Log.InfoFormat("Starting task: {0}", taskToRun.Task.GetType());

            var result = taskToRun.Task.RunTask(taskToRun.Parameters);
            if (result.TaskFinished)
            {
                if (result.Sucessfull)
                {
                    taskToRun.MarkAsFinishedOk(DateTime.Now);
                    Log.InfoFormat("Finished OK: {0}", taskToRun.Task.GetType());
                }
                else
                {
                    taskToRun.MarkAsFinishedError(DateTime.Now);
                    Log.ErrorFormat("Finished with error: {0}", taskToRun.Task.GetType());
                }
            }
        }

        void UpdateEnquedTasks()
        {
            Log.Info("Loading tasks...");
            if (_enquedTasks == null) _enquedTasks = new Dictionary<Guid, EnquedTask>();

            DllLoadState dllLoadState = null;

            foreach (var item in _dllLoadedList)
                item.Value.FilePresenceConfirmed = false;

            try
            {
                if (!System.IO.Directory.Exists(_parameter.TaskLibrary)) System.IO.Directory.CreateDirectory(_parameter.TaskLibrary);

                var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _parameter.TaskLibrary);
                if (System.IO.Directory.Exists(path))
                {
                    Log.InfoFormat("Task .dll directoryu: {0}", path);
                    var dllFileList = System.IO.Directory.GetFiles(path, "*.dll");

                    foreach (var file in dllFileList)
                    {
                        Log.InfoFormat("Investigating file: {0}", System.IO.Path.GetFileName(file));

                        if (!_dllLoadedList.ContainsKey(file))
                        {
                            dllLoadState = new DllLoadState()
                            {
                                DllName = file,
                                DllFileName = file,
                                FilePresenceConfirmed = true
                            };
                        }
                        else
                        {
                            dllLoadState = _dllLoadedList[file];
                            dllLoadState.FilePresenceConfirmed = true;
                        }

                        var res = _dllLoadedList.Count(x => (x.Value.DllName == file) && (x.Value.LoadError));
                        if (res > 0) continue;

                        var dll = Assembly.LoadFile(file);

                        foreach (Type dlltype in dll.GetExportedTypes())
                        {
                            var loadedTaskType = dlltype.GetTypeInfo();

                            var ilist = loadedTaskType.ImplementedInterfaces;

                            if (ilist.Count(z => z.Name.ToLower().Contains("sharptaskinterface")) < 1) continue;

                            var sh = (ISharpTask)Activator.CreateInstance(dlltype);

                            EnquedTask et = new EnquedTask(sh);
                            if (!_enquedTasks.ContainsKey(loadedTaskType.GUID))
                            {
                                _enquedTasks.Add(loadedTaskType.GUID, et);
                                Log.InfoFormat("  Task added: {0} / {1}", loadedTaskType.GUID.ToString(), loadedTaskType);
                                foreach (var tt in sh.RunTrigger)
                                {
                                    var name = tt.Name ?? "Unnamed";
                                    var vtd = tt.TriggerDate.ToString();
                                    var vtt = tt.TriggerTime?.ToString() ?? "";
                                    Log.DebugFormat("    Trigger: {0} / {1} {2}", name, vtd, vtt);
                                    dllLoadState.ConfirmedLoaded = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Log.WarnFormat("Task .dll directory can not be found: {0}", path);
                }

                _lastDllLoad = DateTime.Now;

            }
            catch (Exception ex)
            {
                dllLoadState.LoadError = true;
                if (!_dllLoadedList.ContainsKey(dllLoadState.DllName))
                {
                    _dllLoadedList.Add(dllLoadState.DllName, dllLoadState);
                }
                Log.WarnFormat("Could not load task .DLL file: {0}", dllLoadState.DllFileName);
                Log.Warn("Loading .DLL failes",ex);
            }

            Log.InfoFormat("Task loaded: ");
            foreach (var item in _dllLoadedList.Where(x => !x.Value.ConfirmedLoaded && !x.Value.ReportedToGui))
            {
                Log.InfoFormat("  {0}", item.Value.DllName);
                item.Value.ReportedToGui = true;
            }

            Log.InfoFormat("Task unloaded:");
            foreach (var item in _dllLoadedList.Where(x => !x.Value.LoadError && !x.Value.ReportedToGui))
            {
                Log.InfoFormat("  {0}", item.Value.DllName);
                item.Value.ReportedToGui = true;
            }

            Log.InfoFormat("DLL files removed:");
            var filelist = _dllLoadedList.Where(x => !x.Value.FilePresenceConfirmed).ToList();
            foreach(var item in filelist)
            {
                _dllLoadedList.Remove(item.Value.DllName);
                Log.InfoFormat("  {0}", item.Value.DllName);
            }

        }
    }
}
