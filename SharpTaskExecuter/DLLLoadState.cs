using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskExecuter
{
    public class DLLLoadState
    {
        public string DLLName;
        public bool LoadError;
        public bool ReportedToGui;
        public bool ConfirmedLoaded;
        public string DLLFileName;
        public bool FilePresenceConfirmed;
    }
}
