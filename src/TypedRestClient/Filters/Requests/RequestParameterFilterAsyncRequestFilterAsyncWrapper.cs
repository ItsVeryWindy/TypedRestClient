using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Requests
{
    internal class RequestParameterFilterAsyncRequestFilterAsyncWrapper : IRequestFilterAsync
    {
        private readonly IRequestParameterFilterAsync _filter;
        private readonly string _parameterName;

        public RequestParameterFilterAsyncRequestFilterAsyncWrapper(IRequestParameterFilterAsync filter, string parameterName)
        {
            _filter = filter;
            _parameterName = parameterName;
        }

        public Task OnRequestAsync(IRequestEventArgs eventArgs, CancellationToken cancellationToken)
        {
            var value = eventArgs.Parameters[_parameterName];

            var args = new RequestParameterEventArgs(_parameterName, value, eventArgs.Request);

            return _filter.OnRequestAsync(args, cancellationToken);
        }
    }
}
