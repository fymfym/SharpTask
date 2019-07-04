using System;
using Microsoft.Extensions.Configuration;

namespace SharpTask.Core.Cli.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Func<IServiceProvider, T> ConfigurationFactory<T>(this IConfiguration configuration, string configurationSection) where T : class, new() 
        {
            return factory =>
            {
                var configurationType = new T();

                configuration.GetSection(configurationSection).Bind(configurationType);

                return configurationType;
            };
        }

    }
}
