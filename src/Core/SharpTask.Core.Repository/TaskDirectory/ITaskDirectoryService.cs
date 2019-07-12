using System.IO;

namespace SharpTask.Core.Repository.TaskDirectory
{
    public interface ITaskDirectoryService
    {
        long GetDirectoryHash(DirectoryInfo directory);
    }
}
