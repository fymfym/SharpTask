using System.Collections.Generic;
using System.Linq;

namespace SharpTaskExecuter
{
    public class SharpTaskExecuterParameter
    {
        public string TaskLibrary;

        public SharpTaskExecuterParameter()
        {
            TaskLibrary = ".";
        }

        static string GetParameterValue(List<string> keyValueList, string name, string defaultVaue)
        {
            var idx = keyValueList.IndexOf(name);
            if (idx >= 0)
                if (keyValueList.Count >= idx + 1)
                    return keyValueList[idx + 1];
            return defaultVaue;
        }

        public static SharpTaskExecuterParameter ParseArgs(string[] args)
        {
            var param = new SharpTaskExecuterParameter();
            List<string> lst = args.ToList();

            param.TaskLibrary = GetParameterValue(lst, "tasklibrary", @".");            

            return param;
        }
    }
}
