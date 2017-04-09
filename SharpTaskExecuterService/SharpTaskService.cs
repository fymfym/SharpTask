using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace SharpTaskExecuterService
{
    public partial class SharpTaskService : ServiceBase
    {
        SharpTaskExecuter.SharpTaskExecuter _executer;

        public SharpTaskService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var Param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);

            var logger = SharpTaskExecuter.SharpTaskExecuter.GetLogger(Param);
            logger.Info("Logger'{0}' instantiated", logger.GetType().ToString());

            _executer = new SharpTaskExecuter.SharpTaskExecuter(logger);
            _executer.Start(Param);

        }

        protected override void OnStop()
        {
            _executer.Stop();
        }
    }
}
