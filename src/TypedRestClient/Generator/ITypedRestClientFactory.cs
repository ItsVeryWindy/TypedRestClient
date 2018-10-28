using System;
using System.Net.Http;

namespace TypedRestClient.Generator
{
    public interface ITypedRestClientFactory<TInterface>
    {
        TInterface Create(HttpClient client, TypedRestClientConfiguration configuration);
    }

    public static class ITypedRestClientFactoryExtensions
    {
        public static TInterface Create<TInterface>(this ITypedRestClientFactory<TInterface> factory, HttpClient client)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return factory.Create(client, new TypedRestClientConfiguration());
        }
    }
}
