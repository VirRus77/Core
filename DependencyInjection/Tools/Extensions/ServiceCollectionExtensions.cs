using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Tools.Extensions
{
    /// <summary>
    /// Метод расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавить конфигурацию в коллекцию.
        /// </summary>
        /// <param name="serviceCollection">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <returns>Коллекцию сервисов.</returns>
        public static IServiceCollection AddConfiguration(
            this IServiceCollection serviceCollection,
            [NotNull] IConfigurationRoot configuration
        )
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            serviceCollection
                .AddOptions()
                .AddSingleton<IConfigurationRoot>(configuration)
                .AddSingleton<IConfiguration>(provider => provider.GetService<IConfigurationRoot>());

            return serviceCollection;
        }
    }
}