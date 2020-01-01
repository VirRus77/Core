using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Tools.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T CreateInstance<T>(this IServiceProvider serviceProvider, params object[] arguments)
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, arguments);
        }
    }
}
