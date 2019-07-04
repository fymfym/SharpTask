using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SharpTask.Models;
using SharpTask.Models.Configuration;
using SharpTask.Repository.TaskModule;

namespace SharpTask.Services.TaskDirectoryManipulation
{
    public class TaskManipulationService :ITaskManipulationService
    {

        private readonly TaskDirectoryManipulationConfiguration _configuration;
        private readonly ITaskModuleRepository _taskModuleRepository;

        public TaskManipulationService(
            TaskDirectoryManipulationConfiguration configuration,
            ITaskModuleRepository taskModuleRepository
            )
        {
            _configuration = configuration;
            _taskModuleRepository = taskModuleRepository;
        }

        public IEnumerable<TaskModuleInformation> GetPickupTasks()
        {

            return GetFiles(_configuration.TaskPickupFolder);
        }

        public IEnumerable<TaskModuleInformation> GetRunTasks()
        {
            return GetFiles(_configuration.TaskRunFolder);
        }

        private IEnumerable<TaskModuleInformation> GetFiles(string directory)
        {
            var result = new List<TaskModuleInformation>();

            var list = _taskModuleRepository.GetDirectoryInfo(directory).GetFiles();

            foreach (var item in list)
            {
                var file = _taskModuleRepository.GetFileInfo(item.FullName);
                result.Add(new TaskModuleInformation()
                {
                    Name = file.Name,
                    Hash = GetFileHashCode(file)
                });
            }

            return result;
        }

        private int GetFileHashCode(FileInfo file)
        {
            return file.Name.GetHashCode() +
                   file.CreationTime.ToString(CultureInfo.InvariantCulture).GetHashCode();
        }
    }
}
