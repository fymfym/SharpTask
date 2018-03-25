using System.ServiceProcess;

namespace SharpTaskExecuterService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new SharpTaskService()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
