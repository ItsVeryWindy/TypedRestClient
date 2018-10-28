using System.Net.Http;
using TypedRestClient.Filters.Requests;

namespace TypedRestClient.Attributes
{
    public class HttpHeadAttribute : HttpRouteAttribute
    {
        public HttpHeadAttribute(string route) : base(route)
        {
        }

        public override void OnRequest(IRequestEventArgs eventArgs)
        {
            base.OnRequest(eventArgs);

            eventArgs.Request.Method = HttpMethod.Head;
        }
    }
}
