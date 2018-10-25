using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1.Filters
{
    public interface IResponseFilter
    {
        void OnResponse<TReturn>(IResponseEventArgs<TReturn> eventArgs);
    }

    public interface IResponseFilter<in TConstructorParameters>
    {
        void OnResponse<TReturn>(IResponseEventArgs<TConstructorParameters, TReturn> eventArgs);
    }

    public interface IResponseFilterAsync
    {
        Task OnResponseAsync<TReturn>(IResponseEventArgs<TReturn> eventArgs, CancellationToken cancellationToken);
    }

    public interface IResponseFilterAsync<in TConstructorParameters>
    {
        Task OnResponseAsync<TReturn>(IResponseEventArgs<TConstructorParameters, TReturn> eventArgs, CancellationToken cancellationToken);
    }

    public class ResponseFilterWrapper<TConstructorParameters> : IResponseFilter<TConstructorParameters>
    {
        private readonly IResponseFilter _filter;

        public ResponseFilterWrapper(IResponseFilter filter)
        {
            _filter = filter;
        }

        public void OnResponse<TReturn>(IResponseEventArgs<TConstructorParameters, TReturn> eventArgs)
            => _filter.OnResponse(eventArgs);
    }

    public class ResponseFilterWrapperAsync<TConstructorParameters> : IResponseFilterAsync<TConstructorParameters>
    {
        private readonly IResponseFilter<TConstructorParameters> _filter;

        public ResponseFilterWrapperAsync(IResponseFilter<TConstructorParameters> filter)
        {
            _filter = filter;
        }

        public Task OnResponseAsync<TReturn>(IResponseEventArgs<TConstructorParameters, TReturn> eventArgs, CancellationToken cancellationToken)
        {
            _filter.OnResponse(eventArgs);
            return Task.CompletedTask;
        }
    }

    public class ResponseFilterAsyncWrapper<TConstructorParameters> : IResponseFilterAsync<TConstructorParameters>
    {
        private readonly IResponseFilterAsync _filter;

        public ResponseFilterAsyncWrapper(IResponseFilterAsync filter)
        {
            _filter = filter;
        }

        public Task OnResponseAsync<TReturn>(IResponseEventArgs<TConstructorParameters, TReturn> eventArgs, CancellationToken cancellationToken)
            => _filter.OnResponseAsync(eventArgs, cancellationToken);
    }
}
