using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpTask.Core.Models.Task;
using SharpTask.Core.Services.TaskCollection;

namespace SharpTask.Core.Services.TaskExecuter
{
    public class TaskExecuterService : ITaskExecuterService
    {
        private ILogger<TaskExecuterService> _logger;
        private readonly ITaskCollectionService _taskCollectionService;

        Dictionary<Guid, EnquedTask> _enquedTasks;
        readonly SharpTaskExecuterParameter _parameter;
        bool _running;
        DateTime _lastDllLoad;
        Dictionary<string, DllLoadState> _dllLoadedList;

        public TaskExecuterService(
            ILogger<TaskExecuterService> logger,
            ITaskCollectionService taskCollectionService
            )
        {
            _logger = logger;
            _taskCollectionService = taskCollectionService;
        }

        public void Start()
        {
            _logger.LogInformation("Starting SharpTaskExecuter");
            _running = true;

            while (_running)
            {
                _taskCollectionService.SynchronizeDirectories();

                var tasks = _taskCollectionService.GetRunnableTask();
            }
            
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
            _logger.LogInformation("Waiting for enqued tasks start trigger...");
            while (_running)
            {
                foreach (var task in _enquedTasks)
                {
                    var now = DateTime.Now;
                    var res = task.Value.ShouldExecuteNow(now);
                    if (res.ShouldExecuteNow)
                    {
                        _logger.LogInformation($"Marking task {task.Value} as stared");
                        _logger.LogInformation(" - using triger {0}", res.UsedTrigger.Name ?? "Unnamed");
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
            _logger.LogInformation("Enqued tasks stopped!");
        }

        void _runTask(object taskToRunObject)
        {
            EnquedTask taskToRun = (EnquedTask)taskToRunObject;
            _logger.LogInformation("Starting task: {0}", taskToRun.Task.GetType());

            var result = taskToRun.Task.RunTask(taskToRun.Parameters);
            if (result.TaskFinished)
            {
                if (result.Sucessfull)
                {
                    taskToRun.MarkAsFinishedOk(DateTime.Now);
                    _logger.LogInformation("Finished OK: {0}", taskToRun.Task.GetType());
                }
                else
                {
                    taskToRun.MarkAsFinishedError(DateTime.Now);
                    _logger.LogInformation("Finished with error: {0}", taskToRun.Task.GetType());
                }
            }
        }

        void UpdateEnquedTasks()
        {
            _logger.LogInformation("Loading tasks...");
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
                    _logger.LogInformation("Task .dll directoryu: {0}", path);
                    var dllFileList = System.IO.Directory.GetFiles(path, "*.dll");

                    foreach (var file in dllFileList)
                    {
                        _logger.LogInformation("Investigating file: {0}", System.IO.Path.GetFileName(file));

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
                                _logger.LogInformation("  Task added: {0} / {1}", loadedTaskType.GUID.ToString(), loadedTaskType);
                                foreach (var tt in sh.RunTrigger)
                                {
                                    var name = tt.Name ?? "Unnamed";
                                    var vtd = tt.TriggerDate.ToString();
                                    var vtt = tt.TriggerTime?.ToString() ?? "";
                                    _logger.LogDebug("    Trigger: {0} / {1} {2}", name, vtd, vtt);
                                    dllLoadState.ConfirmedLoaded = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Task .dll directory can not be found: {0}", path);
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
                _logger.LogWarning("Could not load task .DLL file: {0}", dllLoadState.DllFileName);
                _logger.LogWarning("Loading .DLL failes",ex);
            }

            _logger.LogInformation("Task loaded: ");
            foreach (var item in _dllLoadedList.Where(x => !x.Value.ConfirmedLoaded && !x.Value.ReportedToGui))
            {
                _logger.LogInformation("  {0}", item.Value.DllName);
                item.Value.ReportedToGui = true;
            }

            _logger.LogInformation("Task unloaded:");
            foreach (var item in _dllLoadedList.Where(x => !x.Value.LoadError && !x.Value.ReportedToGui))
            {
                _logger.LogInformation("  {0}", item.Value.DllName);
                item.Value.ReportedToGui = true;
            }

            _logger.LogInformation("DLL files removed:");
            var filelist = _dllLoadedList.Where(x => !x.Value.FilePresenceConfirmed).ToList();
            foreach(var item in filelist)
            {
                _dllLoadedList.Remove(item.Value.DllName);
                _logger.LogInformation("  {0}", item.Value.DllName);
            }

        }
    }

}
