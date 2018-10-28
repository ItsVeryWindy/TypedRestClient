using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Responses
{
    internal class ResponseEventArgs<TReturn> : IResponseEventArgs<TReturn>
    {
        public IReadOnlyDictionary<string, object> Parameters { get; }
        public HttpResponseMessage Response { get; }
        public Task<TReturn> ReturnValue { get; set; }

        public ResponseEventArgs(IReadOnlyDictionary<string, object> parameters, HttpResponseMessage response, Task<TReturn> returnValue)
        {
            Parameters = parameters;
            Response = response;
            ReturnValue = returnValue;
        }
    }
}
