using System.IO;

namespace SharpTask.Core.Test.ServicesTest
{
    public class TaskExecuterServiceTest
    {

        private readonly string _pickupFolder;
        private readonly string _runFolder;
        private readonly string _errorFolder;
        private readonly string _unloadFolder;

        private const string PickupFile = "PickupFile1.txt";
        private const string RunFile = "RunFile1.txt";

        public TaskExecuterServiceTest()
        {
            var path = Path.GetTempPath();
            _pickupFolder = path + "/SharpTaskPickup/";
            _runFolder = path + "/SharpTaskRun/";
            _errorFolder = path + "/SharpTaskError/";
            _unloadFolder = path + "/SharpTaskUnload/";

            new FileInfo(_pickupFolder + PickupFile).Create();
            new FileInfo(_pickupFolder + RunFile).Create();

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
    }
}
