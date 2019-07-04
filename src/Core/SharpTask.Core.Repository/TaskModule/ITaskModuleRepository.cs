using System.IO;
using System.Threading.Tasks;

namespace SharpTask.Core.Repository.TaskModule
{
    public interface ITaskModuleRepository
    {
        DirectoryInfo GetDirectoryInfo(string directoryName);
        FileInfo GetFileInfo(string filename);

        Task<FileInfo> MoveFile(FileInfo sourceFile, DirectoryInfo destination);
        Task<FileInfo> CopyFile(FileInfo sourceFile, DirectoryInfo destination);
    }
}
