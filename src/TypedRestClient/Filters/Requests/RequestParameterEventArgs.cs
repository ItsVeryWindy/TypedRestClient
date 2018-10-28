using System.Net.Http;

namespace TypedRestClient.Filters.Requests
{
    internal class RequestParameterEventArgs : IRequestParameterEventArgs
    {
        public string ParameterName { get; }
        public object ParameterValue { get; }
        public HttpRequestMessage Request { get; }

        public RequestParameterEventArgs(string parameterName, object parameterValue, HttpRequestMessage request)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
            Request = request;
        }
    }
}
