using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.Configuration;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Repository.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public class TaskDirectoryManipulationService :ITaskDirectoryManipulationService
    {

        private enum EDirectoryType
        {
            Pickup,
            Run
        }

        private readonly TaskDirectoryManipulationConfiguration _configuration;
        private readonly ITaskModuleRepository _taskModuleRepository;
        private readonly ILogger<TaskDirectoryManipulationService> _logger;

        public TaskDirectoryManipulationService(
            ILogger<TaskDirectoryManipulationService> logger,
            TaskDirectoryManipulationConfiguration configuration,
            ITaskModuleRepository taskModuleRepository
            )
        {
            _configuration = configuration;
            _taskModuleRepository = taskModuleRepository;
            _logger = logger;
        }

        public IEnumerable<TaskInformation> GetTasksInPickupFolder()
        {
            return GetDirectories(_configuration.TaskPickupFolder, EDirectoryType.Pickup);
        }

        public IEnumerable<TaskInformation> GetTasksInRunFolder()
        {
            return GetDirectories(_configuration.TaskRunFolder, EDirectoryType.Run);
        }

        private IEnumerable<TaskInformation> GetDirectories(string directory,EDirectoryType directoryType)
        {
            var result = new List<TaskInformation>();

            var dirInfo = new DirectoryInfo(directory);
            if (!dirInfo.Exists) dirInfo.Create();

            var list = _taskModuleRepository.GetDirectoryInfo(dirInfo.FullName).GetDirectories();

            foreach (var item in list)
            {
                var info = GenerateTaskModuleInformation(item);
                switch (directoryType)
                {
                    case EDirectoryType.Pickup:
                        info.PickupDirectory = item.FullName;
                        break;
                    case EDirectoryType.Run:
                        info.RunDirectory = item.FullName;
                        break;
                }
                result.Add(info);
            }

            return result;
        }


        private TaskInformation GenerateTaskModuleInformation(DirectoryInfo info)
        {
            return new TaskInformation()
            {
                TaskDirectoryName = info.Name,
                Hash = GetDirectoryHashCode(info)
            };
        }


        private static int GetDirectoryHashCode(DirectoryInfo directory)
        {
            return directory.Name.GetHashCode() +
                   directory.CreationTime.ToString(CultureInfo.InvariantCulture).GetHashCode();
        }

        public async Task<TaskInformation> CopyTaskFromPickupToRunFolder(TaskInformation taskInformation)
        {
            var dest = new DirectoryInfo(Path.Combine(_configuration.TaskRunFolder,taskInformation.TaskDirectoryName));
            dest.Create();

            if (string.IsNullOrEmpty(taskInformation.PickupDirectory)) throw new Exception("Pickup folder value is empty, should not be");
            var dir = new DirectoryInfo(taskInformation.PickupDirectory);

            foreach (var fileInfo in dir.GetFiles())
            {
                await _taskModuleRepository.CopyFile(fileInfo, dest);
            }

            taskInformation.RunDirectory = dest.FullName;

            _logger.LogInformation("{@action}{@source}{@destination}",
                "CopyTaskFromPickupToRunFolder",
                taskInformation,
                dest);

            return taskInformation;
        }

        public async Task<TaskInformation> MoveTaskFromPickupToErrorFolder(TaskInformation taskInformation)
        {
            var dest = new DirectoryInfo(Path.Combine(_configuration.TaskLoadErrorFolder,taskInformation.TaskDirectoryName));
            dest.Create();

            if (string.IsNullOrEmpty(taskInformation.PickupDirectory)) throw new Exception("Pickup folder value is empty, should not be");
            var dir = new DirectoryInfo(taskInformation.PickupDirectory);

            foreach (var fileInfo in dir.GetFiles())
            {
                await _taskModuleRepository.CopyFile(fileInfo, dest);
            }

            _logger.LogInformation("{@action}{@source}{@destination}",
                "MoveTaskFromPickupToErrorFolder",
                taskInformation,
                dest.FullName);

            return taskInformation;
        }

        public async  Task<TaskInformation> MoveTaskFromRunToUnloadFolder(TaskInformation taskInformation)
        {
            var dest = new DirectoryInfo(Path.Combine(_configuration.TaskUnloadFolder,taskInformation.TaskDirectoryName));
            dest.Create();

            if (string.IsNullOrEmpty(taskInformation.RunDirectory)) throw new Exception("Run folder value is empty, should not be");

            var dir = new DirectoryInfo(taskInformation.PickupDirectory);

            foreach (var fileInfo in dir.GetFiles())
            {
                await _taskModuleRepository.CopyFile(fileInfo, dest);
            }

            _logger.LogInformation("{@action}{@source}{@destination}",
                "MoveTaskFromRunToUnloadFolder",
                taskInformation,
                dest.FullName);

            return taskInformation;
        }
    }
}
