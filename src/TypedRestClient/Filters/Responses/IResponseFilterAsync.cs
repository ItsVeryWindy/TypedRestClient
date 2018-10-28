using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Responses
{
    public interface IResponseFilterAsync
    {
        Task OnResponseAsync<TReturn>(IResponseEventArgs<TReturn> eventArgs, CancellationToken cancellationToken);
    }
}
