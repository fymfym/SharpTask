using System.ServiceProcess;
using log4net;

namespace SharpTaskExecuterService
{
    public partial class SharpTaskService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SharpTaskService));

        private SharpTaskExecuter.SharpTaskExecuter _executer;

        public SharpTaskService()
        {
            Log.Info("Service construct");
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Service: On start");
            var param = SharpTaskExecuter.SharpTaskExecuterParameter.ParseArgs(args);

            _executer = new SharpTaskExecuter.SharpTaskExecuter(param);
            _executer.Start();

        }

        protected override void OnStop()
        {
            Log.Info("Service: On stop");
            _executer.Stop();
        }
    }
}
