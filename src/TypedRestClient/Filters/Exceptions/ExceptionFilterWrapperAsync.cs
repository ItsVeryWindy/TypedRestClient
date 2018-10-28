using System.Threading;
using System.Threading.Tasks;

namespace TypedRestClient.Filters.Exceptions
{
    internal class ExceptionFilterWrapperAsync : IExceptionFilterAsync
    {
        private readonly IExceptionFilter _filter;

        public ExceptionFilterWrapperAsync(IExceptionFilter filter)
        {
            _filter = filter;
        }

        public Task OnExceptionAsync(IExceptionEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _filter.OnException(eventArgs);

            return Task.CompletedTask;
        }
    }
}
