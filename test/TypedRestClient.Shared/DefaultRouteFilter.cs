using System;
using TypedRestClient.Filters.Requests;

namespace TypedRestClient.Shared
{
    public class DefaultRouteFilter : IRequestFilter
    {
        public void OnRequest(IRequestEventArgs eventArgs)
        {
            eventArgs.Request.RequestUri = new Uri("/", UriKind.Relative);
        }
    }
}
