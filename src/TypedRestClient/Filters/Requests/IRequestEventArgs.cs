using System.Collections.Generic;
using System.Net.Http;

namespace TypedRestClient.Filters.Requests
{
    public interface IRequestEventArgs
    {
        IReadOnlyDictionary<string, object> Parameters { get; }
        HttpRequestMessage Request { get; }
    }
}
