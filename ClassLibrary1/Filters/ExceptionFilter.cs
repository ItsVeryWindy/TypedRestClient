using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1.Filters
{
    public interface IExceptionFilter
    {
        void OnException(IExceptionEventArgs eventArgs);
    }

    public interface IExceptionFilter<in TConstructorParameters>
    {
        void OnException(IExceptionEventArgs<TConstructorParameters> eventArgs);
    }

    public interface IExceptionFilterAsync
    {
        Task OnExceptionAsync(IExceptionEventArgs eventArgs, CancellationToken cancellationToken);
    }

    public interface IExceptionFilterAsync<in TConstructorParameters>
    {
        Task OnExceptionAsync(IExceptionEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken);
    }

    public class ExceptionFilterWrapper<TConstructorParameters> : IExceptionFilter<TConstructorParameters>
    {
        private readonly IExceptionFilter _filter;

        public ExceptionFilterWrapper(IExceptionFilter filter)
        {
            _filter = filter;
        }

        public void OnException(IExceptionEventArgs<TConstructorParameters> eventArgs)
            => _filter.OnException(eventArgs);
    }

    public class ExceptionFilterWrapperAsync<TConstructorParameters> : IExceptionFilterAsync<TConstructorParameters>
    {
        private readonly IExceptionFilter<TConstructorParameters> _filter;

        public ExceptionFilterWrapperAsync(IExceptionFilter<TConstructorParameters> filter)
        {
            _filter = filter;
        }

        public Task OnExceptionAsync(IExceptionEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken)
        {
            _filter.OnException(eventArgs);
            return Task.CompletedTask;
        }
    }

    public class ExceptionFilterAsyncWrapper<TConstructorParameters> : IExceptionFilterAsync<TConstructorParameters>
    {
        private readonly IExceptionFilterAsync _filter;

        public ExceptionFilterAsyncWrapper(IExceptionFilterAsync filter)
        {
            _filter = filter;
        }

        public Task OnExceptionAsync(IExceptionEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken)
            => _filter.OnExceptionAsync(eventArgs, cancellationToken);
    }
}
