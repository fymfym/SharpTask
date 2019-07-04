using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpTask.Repository.TaskModule
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
    }
}
