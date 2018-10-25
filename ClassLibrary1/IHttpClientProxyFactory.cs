using System.Net.Http;

namespace ClassLibrary1
{
    public interface IHttpClientProxyFactory<TInterface, TAdditionalConstructorParameters>
    {
        TInterface Create(HttpClient httpClient, TAdditionalConstructorParameters parameters);
    }
}
