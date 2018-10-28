using System.Net.Http;

namespace TypedRestClient.Filters.Requests
{
    public interface IRequestParameterEventArgs
    {
        string ParameterName { get; }
        object ParameterValue { get; }
        HttpRequestMessage Request { get; }
    }
}
