using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Requests
{
    public interface IRequestParameterFilterAsync
    {
        Task OnRequestAsync(IRequestParameterEventArgs eventArgs, CancellationToken cancellationToken);
    }
}
