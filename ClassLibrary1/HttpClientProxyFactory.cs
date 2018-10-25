using System;
using System.Net.Http;

namespace ClassLibrary1
{
    public class HttpClientProxyFactory<TInterface, TAdditionalConstructorParameters> : IHttpClientProxyFactory<TInterface, TAdditionalConstructorParameters>
    {
        private readonly Type _type;
        private readonly IHttpClientProxy<TAdditionalConstructorParameters> _httpClientProxy;

        public HttpClientProxyFactory(Type type, IHttpClientProxy<TAdditionalConstructorParameters> httpClientProxy)
        {
            _type = type;
            _httpClientProxy = httpClientProxy;
        }

        public TInterface Create(HttpClient httpClient, TAdditionalConstructorParameters parameters)
        {
            return (TInterface)Activator.CreateInstance(_type, _httpClientProxy, httpClient, parameters);
        }
    }
}
