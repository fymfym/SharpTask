using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using SharpTask.Core.Cli.Extensions;
using SharpTask.Core.Repository.TaskModule;
using SharpTask.Core.Services.TaskCollection;
using SharpTask.Core.Services.TaskDirectoryManipulation;
using SharpTask.Core.Services.TaskDllLoader;
using SharpTask.Core.Services.TaskExecuter;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SharpTask.Core.Cli
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                var task = BuildCliHost().RunConsoleAsync();
                Task.WaitAll(task);
            }
            catch (Exception e)
            {
                logger.Error($"Attp transfer exception:{e}");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static IHostBuilder BuildCliHost()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                {
                    ApplyApplicationConfiguration(configurationBuilder);
                })
                .ConfigureLogging((hostBuilderContext, loggingBuilder) => { SetUpLogging(loggingBuilder); })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    BuildServiceCollection(serviceCollection);
                    serviceCollection.AddHostedService<CliExecutor>();
                })
                .UseNLog()
                .UseDirectoryConfiguration();

            return hostBuilder;
        }


        private static void ApplyApplicationConfiguration(
            IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json", optional: false);
        }

        private static void SetUpLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        }


        private static void BuildServiceCollection(
            IServiceCollection serviceCollection
            )
        {
            //serviceCollection.AddHttpClient<SomeHttpClient>();

            serviceCollection.AddTransient<ITaskDllLoaderService,TaskDllLoaderService>();
            serviceCollection.AddTransient<ITaskModuleRepository,TaskModuleRepository>();
            serviceCollection.AddTransient<ITaskExecuterService,TaskExecuterService>();
            serviceCollection.AddTransient<ITaskCollectionService,TaskCollectionService>();
            serviceCollection.AddTransient<ITaskDirectoryManipulationService,TaskDirectoryManipulationService>();
            
        }

    }
}
