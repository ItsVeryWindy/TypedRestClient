using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Responses
{
    internal class ResponseFilterWrapperAsync : IResponseFilterAsync
    {
        private readonly IResponseFilter _filter;

        public ResponseFilterWrapperAsync(IResponseFilter filter)
        {
            _filter = filter;
        }

        public Task OnResponseAsync<TReturn>(IResponseEventArgs<TReturn> eventArgs, CancellationToken cancellationToken)
        {
            _filter.OnResponse(eventArgs);

            return Task.CompletedTask;
        }
    }
}
