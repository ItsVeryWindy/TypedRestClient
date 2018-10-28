using System;

namespace TypedRestClient.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static TypedRestClientConfiguration AddServiceProvider(this TypedRestClientConfiguration configuration, IServiceProvider serviceProvider)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            configuration.Get<DependencyInjectionConfiguration>().ServiceProvider = serviceProvider;

            return configuration;
        }
    }
}
