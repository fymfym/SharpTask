using System.IO;
using System.Linq;
using SharpTask.Core.Models.Configuration;
using SharpTask.Core.Repository.TaskModule;
using Xunit;

namespace SharpTask.Core.Test.Services
{
    public class TaskExecuterServiceTest
    {

        private readonly string _pickupFolder;
        private readonly string _runFolder;
        private readonly string _errorFolder;
        private readonly string _unloadFolder;

        private readonly TaskDirectoryManipulationConfiguration _configuration;
        private readonly ITaskModuleRepository _repo;

        private const string PickupFile = "PickupFile1.txt";
        private const string RunFile = "RunFile1.txt";

        public TaskExecuterServiceTest()
        {
            _pickupFolder = System.IO.Path.GetTempPath() + "/SharpTaskPickup/";
            _runFolder = System.IO.Path.GetTempPath() + "/SharpTaskRun/";
            _errorFolder = System.IO.Path.GetTempPath() + "/SharpTaskError/";
            _unloadFolder = System.IO.Path.GetTempPath() + "/SharpTaskUnload/";

            new FileInfo(_pickupFolder + PickupFile).Create();
            new FileInfo(_pickupFolder + RunFile).Create();

            _configuration = new TaskDirectoryManipulationConfiguration()
            {
                TaskLoadErrorFolder = _errorFolder,
                TaskPickupFolder = _pickupFolder,
                TaskRunFolder = _runFolder,
                TaskUnloadFolder = _unloadFolder
            };
            _repo = new TaskModuleRepository();
        }

        ~TaskExecuterServiceTest()
        {
            foreach (var file in new DirectoryInfo(_pickupFolder).EnumerateFiles())
                file.Delete();
            new DirectoryInfo(_pickupFolder).Delete();

            foreach (var file in new DirectoryInfo(_runFolder).EnumerateFiles())
                file.Delete();
            new DirectoryInfo(_runFolder).Delete();

            foreach (var file in new DirectoryInfo(_errorFolder ).EnumerateFiles())
                file.Delete();
            new DirectoryInfo(_errorFolder ).Delete();
            
            foreach (var file in new DirectoryInfo(_unloadFolder).EnumerateFiles())
                file.Delete();
            new DirectoryInfo(_unloadFolder).Delete();
        }

        [Fact]
        public void GetTasksInPickupFolderTest()
        {
            var service = new SharpTask.Core.Services.TaskDirectoryManipulation.TaskDirectoryManipulationService(_configuration,_repo);

            var files = service.GetTasksInPickupFolder().ToList();

            Assert.NotEmpty(files);
            Assert.NotEmpty(files.Where(x => x.FullFileName.EndsWith(PickupFile)));

        }

        [Fact]
        public void GetTasksInRunFolderTest()
        {
            var service = new SharpTask.Core.Services.TaskDirectoryManipulation.TaskDirectoryManipulationService(_configuration,_repo);

            var files = service.GetTasksInRunFolder().ToList();

            Assert.NotEmpty(files);
            Assert.NotEmpty(files.Where(x => x.FullFileName.EndsWith(RunFile)));
        }

    }
}
