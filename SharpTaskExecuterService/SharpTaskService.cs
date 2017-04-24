using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using log4net;

namespace SharpTaskExecuterService
{
    public partial class SharpTaskService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharpTaskService));

        SharpTaskExecuter.SharpTaskExecuter _executer;

        public SharpTaskService()
        {
            log.Info("Service construct");
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Service: On start");
            var Param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);

            _executer = new SharpTaskExecuter.SharpTaskExecuter(Param);
            _executer.Start();

        }

        protected override void OnStop()
        {
            log.Info("Service: On stop");
            _executer.Stop();
        }
    }
}
