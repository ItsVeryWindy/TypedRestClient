using System;
using System.Net.Http;

namespace TypedRestClient.Generator
{
    internal class TypedRestClientFactory<TInterface> : ITypedRestClientFactory<TInterface>
    {
        private readonly Type _type;
        private readonly IHttpClientProxy _httpClientProxy;

        public TypedRestClientFactory(Type type, IHttpClientProxy httpClientProxy)
        {
            _type = type;
            _httpClientProxy = httpClientProxy;
        }

        public TInterface Create(HttpClient client, TypedRestClientConfiguration configuration)
        {
            return (TInterface)Activator.CreateInstance(_type, _httpClientProxy, client, configuration);
        }
    }
}
