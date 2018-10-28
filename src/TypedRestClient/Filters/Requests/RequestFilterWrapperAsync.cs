using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Requests
{
    internal class RequestFilterWrapperAsync : IRequestFilterAsync
    {
        private readonly IRequestFilter _filter;

        public RequestFilterWrapperAsync(IRequestFilter filter)
        {
            _filter = filter;
        }

        public Task OnRequestAsync(IRequestEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _filter.OnRequest(eventArgs);

            return Task.CompletedTask;
        } 
    }
}
