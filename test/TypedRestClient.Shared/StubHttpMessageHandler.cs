using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Shared
{
    public class StubHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage Request { get; private set; }
        public HttpResponseMessage Response { get; set; } = new HttpResponseMessage();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;

            return Task.FromResult(Response);
        }
    }
}
