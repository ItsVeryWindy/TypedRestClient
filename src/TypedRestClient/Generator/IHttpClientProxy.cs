using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Generator
{
    public interface IHttpClientProxy
    {
        List<Filters> Initialize(HttpClient client, TypedRestClientConfiguration configuration);

        Task<T> CallClient<T>(HttpClient client, List<Filters> filters, int methodId, object[] parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
