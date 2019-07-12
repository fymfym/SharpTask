using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Models.Configuration;
using SharpTask.Core.Models.TaskModule;
using SharpTask.Core.Repository.TaskModule;

namespace SharpTask.Core.Services.TaskDirectoryManipulation
{
    public class TaskDirectoryManipulationService :ITaskDirectoryManipulationService
    {
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

        public IEnumerable<TaskInformation> GetDirectoriesInPickupFolder()
        {
            return GetDirectories(_configuration.TaskPickupFolder);
        }

        public IEnumerable<TaskInformation> GetDirectoriesInRunFolder()
        {
            return GetDirectories(_configuration.TaskRunFolder);
        }

        private IEnumerable<TaskInformation> GetDirectories(string directory)
        {
            var result = new List<TaskInformation>();

            var dirInfo = new DirectoryInfo(directory);
            if (!dirInfo.Exists) dirInfo.Create();

            var list = _taskModuleRepository.GetDirectoryInfo(dirInfo.FullName).GetDirectories();

            foreach (var item in list)
            {
                var info = new TaskInformation()
                {
                    Directory = item,
                    DirectoryMd5 = GetDirectoryMd5(item)
                };
                result.Add(info);
            }

            return result;
        }

        public string GetDirectoryMd5(DirectoryInfo directory)
        {
            var hash = "";

            foreach (var fileInfo in directory.GetFiles())
            {
                hash += GetFileHash(fileInfo);
            }

            return CalculateMD5Hash(hash);
        }

        private string GetFileHash(FileInfo fileInfo)
        {
            byte[] hash;
            var sha1 = SHA1.Create();
            using (FileStream stream = fileInfo.OpenRead()) 
                hash = sha1.ComputeHash(stream);

            return BitConverter.ToString(hash);
        }

        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
 
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
        public async Task<TaskInformation> CopyDirectoryToRunFolder(TaskInformation taskInformation)
        {
            var dest = new DirectoryInfo(Path.Combine(_configuration.TaskRunFolder,taskInformation.DirectoryName));
            dest.Create();

            foreach (var fileInfo in taskInformation.Directory.GetFiles())
            {
                await _taskModuleRepository.CopyFile(fileInfo, dest);
            }

            _logger.LogInformation("{@action}{@source}{@destination}",
                "CopyDirectoryToRunFolder",
                taskInformation,
                dest);

            return taskInformation;
        }

        public async Task<TaskInformation> MoveDirectoryToErrorFolder(TaskInformation taskInformation)
        {
            var dest = new DirectoryInfo(Path.Combine(_configuration.TaskLoadErrorFolder,taskInformation.Directory.Name));
            dest.Create();

            foreach (var fileInfo in taskInformation.Directory.GetFiles())
            {
                await _taskModuleRepository.CopyFile(fileInfo, dest);
            }

            _logger.LogInformation("{@action}{@source}{@destination}",
                "MoveDirectoryToErrorFolder",
                taskInformation,
                dest.FullName);

            return taskInformation;
        }

        public async  Task<TaskInformation> MoveDirectoryUnloadFolder(TaskInformation taskInformation)
        {
            var dest = new DirectoryInfo(Path.Combine(_configuration.TaskUnloadFolder,taskInformation.Directory.Name));
            dest.Create();

            foreach (var fileInfo in taskInformation.Directory.GetFiles())
            {
                await _taskModuleRepository.CopyFile(fileInfo, dest);
            }

            _logger.LogInformation("{@action}{@source}{@destination}",
                "MoveDirectoryUnloadFolder",
                taskInformation,
                dest.FullName);

            return taskInformation;
        }
    }
}
