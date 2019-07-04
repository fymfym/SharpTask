using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpTask.Repository.TaskModule
{
    public interface ITaskModuleRepository
    {
        DirectoryInfo GetDirectoryInfo(string directoryName);
        FileInfo GetFileInfo(string filename);
    }
}
