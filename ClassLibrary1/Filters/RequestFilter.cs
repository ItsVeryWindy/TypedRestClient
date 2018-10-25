using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1.Filters
{
    public interface IRequestFilter
    {
        void OnRequest(IRequestEventArgs eventArgs);
    }

    public interface IRequestFilter<in TConstructorParameters>
    {
        void OnRequest(IRequestEventArgs<TConstructorParameters> eventArgs);
    }

    public interface IRequestFilterAsync
    {
        Task OnRequestAsync(IRequestEventArgs eventArgs, CancellationToken cancellationToken);
    }

    public interface IRequestFilterAsync<in TConstructorParameters>
    {
        Task OnRequestAsync(IRequestEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken);
    }

    public class RequestFilterWrapper<TConstructorParameters> : IRequestFilter<TConstructorParameters>
    {
        private readonly IRequestFilter _filter;

        public RequestFilterWrapper(IRequestFilter filter)
        {
            _filter = filter;
        }

        public void OnRequest(IRequestEventArgs<TConstructorParameters> eventArgs)
            => _filter.OnRequest(eventArgs);
    }

    public class RequestFilterWrapperAsync<TConstructorParameters> : IRequestFilterAsync<TConstructorParameters>
    {
        private readonly IRequestFilter<TConstructorParameters> _filter;

        public RequestFilterWrapperAsync(IRequestFilter<TConstructorParameters> filter)
        {
            _filter = filter;
        }

        public Task OnRequestAsync(IRequestEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken)
        {
            _filter.OnRequest(eventArgs);
            return Task.CompletedTask;
        } 
    }

    public class RequestFilterAsyncWrapper<TConstructorParameters> : IRequestFilterAsync<TConstructorParameters>
    {
        private readonly IRequestFilterAsync _filter;

        public RequestFilterAsyncWrapper(IRequestFilterAsync filter)
        {
            _filter = filter;
        }

        public Task OnRequestAsync(IRequestEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken)
            => _filter.OnRequestAsync(eventArgs, cancellationToken);
    }
}
