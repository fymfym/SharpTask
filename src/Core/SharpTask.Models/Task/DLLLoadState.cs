
namespace SharpTask.Core.Models.Task
{
    public class DllLoadState
    {
        public string DllName;
        public bool LoadError;
        public bool ReportedToGui;
        public bool ConfirmedLoaded;
        public string DllFileName;
        public bool FilePresenceConfirmed;
    }
}
