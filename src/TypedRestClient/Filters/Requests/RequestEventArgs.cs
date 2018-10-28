using System.Collections.Generic;
using System.Net.Http;

namespace TypedRestClient.Filters.Requests
{
    internal class RequestEventArgs : IRequestEventArgs
    {
        public IReadOnlyDictionary<string, object> Parameters { get; }
        public HttpRequestMessage Request { get; }

        public RequestEventArgs(IReadOnlyDictionary<string, object> parameters, HttpRequestMessage request)
        {
            Parameters = parameters;
            Request = request;
        }
    }
}
