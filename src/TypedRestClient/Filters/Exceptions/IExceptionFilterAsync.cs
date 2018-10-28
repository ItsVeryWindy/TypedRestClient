using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Exceptions
{
    public interface IExceptionFilterAsync
    {
        Task OnExceptionAsync(IExceptionEventArgs eventArgs, CancellationToken cancellationToken);
    }
}
