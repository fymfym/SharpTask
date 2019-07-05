using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpTask.Core.Services.TaskExecuter;

namespace SharpTask.Core.Cli
{
    public class CliExecutor :IHostedService, IDisposable
    {
        private readonly ITaskExecuterService _taskExecuterService;
        private readonly ILogger<CliExecutor> _logger;

        public CliExecutor(
            ILogger<CliExecutor> logger,
            ITaskExecuterService taskExecuterService
            
            )
        {
            _logger = logger;
            _taskExecuterService = taskExecuterService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            _taskExecuterService.Start();
            _logger.LogInformation("Stated");
            return null;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            _taskExecuterService.Stop();
            _logger.LogInformation("Stopped");
            return null;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing");
        }
    }
}
