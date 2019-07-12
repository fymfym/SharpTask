using System;
using System.Collections.Generic;
using System.IO;
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Models.TaskModule
{
    public class TaskInformation
    {
        /// <summary>
        /// The file based directory
        /// </summary>
        public DirectoryInfo Directory;

        /// <summary>
        /// The classes in the directory that can be ran
        /// </summary>
        public IEnumerable<ISharpTask> RunInstance;

        /// <summary>
        /// MD5 checksum for all files in the directory
        /// </summary>
        public string DirectoryMd5;
        
        /// <summary>
        /// The name of the directory, with original directory name and the file MD5 checksum
        /// </summary>
        public string DirectoryName
        {
            get
            {
                var name = Directory.Name;
                var idx = name.LastIndexOf("_MD5_", StringComparison.InvariantCulture);
                if (idx > 0) name = name.Substring(0, idx);

                return name +
                       "_MD5_" +
                       DirectoryMd5;
            }
        }
    }
}
