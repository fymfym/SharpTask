using System;
using System.IO;
using System.Threading.Tasks;

namespace SharpTask.Core.Repository.TaskModule
{
    public class TaskModuleRepository :ITaskModuleRepository
    {
        public DirectoryInfo GetDirectoryInfo(string directoryName)
        {
            return new DirectoryInfo(directoryName);
        }

        public FileInfo GetFileInfo(string filename)
        {
            return new FileInfo(filename);
        }

        public async Task<FileInfo> MoveFile(FileInfo sourceFile, DirectoryInfo destination)
        {
            var newPath = System.IO.Path.Combine(destination.FullName, sourceFile.Name);
            await CopyFile(sourceFile, destination);
            sourceFile.Delete();
            return  new FileInfo(newPath);
        }

        public async Task<FileInfo> CopyFile(FileInfo sourceFile, DirectoryInfo destination)
        {
            var newPath = System.IO.Path.Combine(destination.FullName, sourceFile.Name);

            using (var sourceStream = new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan))
            using (var destinationStream = new FileStream(newPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan))
                await sourceStream.CopyToAsync(destinationStream);
            return  new FileInfo(newPath);
        }

    }
}
