using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpTask.Core.Models.Configuration;

namespace SharpTask.Core.Cli.Extensions
{
    public static class ConfigurationHostBuilderExtensions
    {
        public static IHostBuilder UseDirectoryConfiguration(this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices((hostBuilderContext, services) =>
            {
                var x = hostBuilderContext.Configuration.ConfigurationFactory<TaskDirectoryManipulationConfiguration>(
                    "directories");

                services.AddSingleton(x);
            });

            return builder;
        }

    }
}
