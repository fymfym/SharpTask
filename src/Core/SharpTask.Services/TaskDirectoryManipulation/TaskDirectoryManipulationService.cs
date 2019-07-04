using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using SharpTask.Core.Models.Configuration;
using SharpTask.Core.Repository.TaskModule;
using SharpTask.Models;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public class TaskDirectoryManipulationService :ITaskDirectoryManipulationService
    {

        private readonly TaskDirectoryManipulationConfiguration _configuration;
        private readonly ITaskModuleRepository _taskModuleRepository;

        public TaskDirectoryManipulationService(
            TaskDirectoryManipulationConfiguration configuration,
            ITaskModuleRepository taskModuleRepository
            )
        {
            _configuration = configuration;
            _taskModuleRepository = taskModuleRepository;
        }

        public IEnumerable<TaskModuleInformation> GetTasksInPickupFolder()
        {
            return GetFiles(_configuration.TaskPickupFolder);
        }

        public IEnumerable<TaskModuleInformation> GetTasksInRunFolder()
        {
            return GetFiles(_configuration.TaskRunFolder);
        }

        private IEnumerable<TaskModuleInformation> GetFiles(string directory)
        {
            var result = new List<TaskModuleInformation>();


            var dirInfo = new DirectoryInfo(directory);
            if (!dirInfo.Exists) dirInfo.Create();

            var list = _taskModuleRepository.GetDirectoryInfo(dirInfo.FullName).GetFiles();

            foreach (var item in list)
            {
                var file = _taskModuleRepository.GetFileInfo(item.FullName);
                result.Add(GenerateTaskModuleInformation(file));
            }

            return result;
        }

        private TaskModuleInformation GenerateTaskModuleInformation(FileInfo fileInfo)
        {
            return new TaskModuleInformation()
            {
                FullFileName = fileInfo.FullName,
                Hash = GetFileHashCode(fileInfo)
            };
        }


        private int GetFileHashCode(FileInfo file)
        {
            return file.Name.GetHashCode() +
                   file.CreationTime.ToString(CultureInfo.InvariantCulture).GetHashCode();
        }

        public async Task<TaskModuleInformation> CopyTaskFromPickupToRunFolder(TaskModuleInformation taskInformation)
        {
            var dest = new DirectoryInfo(_configuration.TaskRunFolder);
            var newFile = await _taskModuleRepository.CopyFile(new FileInfo(taskInformation.FullFileName), dest);
            var res = GenerateTaskModuleInformation(newFile);
            return res;
        }

        public async Task<TaskModuleInformation> MoveTaskFromPickupToErrorFolder(TaskModuleInformation taskInformation)
        {
            var dest = new DirectoryInfo(_configuration.TaskLoadErrorFolder);
            var newFile = await _taskModuleRepository.MoveFile(new FileInfo(taskInformation.FullFileName), dest);
            var res = GenerateTaskModuleInformation(newFile);
            return res;
        }

        public async  Task<TaskModuleInformation> MoveTaskFromRunToUnloadFolder(TaskModuleInformation taskInformation)
        {
            var dest = new DirectoryInfo(_configuration.TaskUnloadFolder);
            var newFile = await _taskModuleRepository.MoveFile(new FileInfo(taskInformation.FullFileName), dest);
            var res = GenerateTaskModuleInformation(newFile);
            return res;
        }
    }
}
