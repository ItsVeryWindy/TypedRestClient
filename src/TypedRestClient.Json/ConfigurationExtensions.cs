using Newtonsoft.Json;
using System;

namespace TypedRestClient.Json
{
    public static class ConfigurationExtensions
    {
        public static TypedRestClientConfiguration AddJsonSerializerSettings(this TypedRestClientConfiguration configuration, JsonSerializerSettings settings)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.Get<JsonSerializerSettingsConfiguration>().SerializerSettings = settings;

            return configuration;
        }
    }
}
