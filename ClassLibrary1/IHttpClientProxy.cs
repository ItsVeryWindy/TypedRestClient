using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IHttpClientProxy<in TConstructorParameters>
    {
        Task<T> CallClient<T>(HttpClient httpClient, TConstructorParameters constructorParameters, int methodId, object[] parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
