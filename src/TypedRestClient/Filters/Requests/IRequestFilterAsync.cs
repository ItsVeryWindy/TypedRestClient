using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Requests
{
    public interface IRequestFilterAsync
    {
        Task OnRequestAsync(IRequestEventArgs eventArgs, CancellationToken cancellationToken);
    }
}
