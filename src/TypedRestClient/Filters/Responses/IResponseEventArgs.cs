using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Responses
{
    public interface IResponseEventArgs<TReturn>
    {
        IReadOnlyDictionary<string, object> Parameters { get; }
        HttpResponseMessage Response { get; }
        Task<TReturn> ReturnValue { get; set; }
    }
}
