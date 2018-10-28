using System.Net.Http;
using TypedRestClient.Filters.Requests;

namespace TypedRestClient.Attributes
{
    public class HttpDeleteAttribute : HttpRouteAttribute
    {
        public HttpDeleteAttribute(string route) : base(route)
        {
        }

        public override void OnRequest(IRequestEventArgs eventArgs)
        {
            base.OnRequest(eventArgs);

            eventArgs.Request.Method = HttpMethod.Delete;
        }
    }
}
