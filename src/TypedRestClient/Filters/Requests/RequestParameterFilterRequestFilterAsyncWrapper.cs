using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Requests
{
    internal class RequestParameterFilterRequestFilterAsyncWrapper : IRequestFilterAsync
    {
        private readonly IRequestParameterFilter _filter;
        private readonly string _parameterName;

        public RequestParameterFilterRequestFilterAsyncWrapper(IRequestParameterFilter filter, string parameterName)
        {
            _filter = filter;
            _parameterName = parameterName;
        }

        public Task OnRequestAsync(IRequestEventArgs eventArgs, CancellationToken cancellationToken)
        {
            var value = eventArgs.Parameters[_parameterName];

            var args = new RequestParameterEventArgs(_parameterName, value, eventArgs.Request);

            _filter.OnRequest(args);

            return Task.CompletedTask;
        }
    }
}
