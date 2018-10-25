using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1.Filters
{
    public interface IRequestParameterFilter
    {
        void OnRequest(IRequestParameterEventArgs eventArgs);
    }

    public interface IRequestParameterFilter<in TConstructorParameters>
    {
        void OnRequest(IRequestParameterEventArgs<TConstructorParameters> eventArgs);
    }

    public interface IRequestParameterFilterAsync
    {
        Task OnRequestAsync(IRequestParameterEventArgs eventArgs, CancellationToken cancellationToken);
    }

    public interface IRequestParameterFilterAsync<in TConstructorParameters>
    {
        Task OnRequestAsync(IRequestParameterEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken);
    }

    public class RequestParameterFilterRequestFilterWrapperAsync<TConstructorParameters> : IRequestFilterAsync<TConstructorParameters>
    {
        private readonly IRequestParameterFilter<TConstructorParameters> _filter;
        private readonly string _parameterName;

        public RequestParameterFilterRequestFilterWrapperAsync(IRequestParameterFilter<TConstructorParameters> filter, string parameterName)
        {
            _filter = filter;
            _parameterName = parameterName;
        }

        public Task OnRequestAsync(IRequestEventArgs<TConstructorParameters> eventArgs, CancellationToken cancellationToken)
        {
            var value = eventArgs.Parameters[_parameterName];

            _filter.OnRequest(new RequestParameterEventArgs<TConstructorParameters>(eventArgs.ConstructorParameters, _parameterName, value, eventArgs.Request));

            return Task.CompletedTask;
        }
    }

    public class RequestParameterFilterAsyncRequestFilterAsyncWrapper<TConstructorParameters> : IRequestFilterAsync<TConstructorParameters>
    {
        private readonly IRequestParameterFilterAsync _filter;
        private readonly string _parameterName;

        public RequestParameterFilterAsyncRequestFilterAsyncWrapper(IRequestParameterFilterAsync filter, string parameterName)
        {
            _filter = filter;
            _parameterName = parameterName;
        }

        public Task OnRequestAsync(IRequestEventArgs<TConstructorParameters> eventArgs,
            CancellationToken cancellationToken)
        {
            var value = eventArgs.Parameters[_parameterName];

            return _filter.OnRequestAsync(
                new RequestParameterEventArgs<TConstructorParameters>(eventArgs.ConstructorParameters, _parameterName,
                    value, eventArgs.Request), cancellationToken);
        }
    }

    public class RequestParameterFilterAsyncOfTRequestFilterAsyncWrapper<TConstructorParameters> : IRequestFilterAsync<TConstructorParameters>
    {
        private readonly IRequestParameterFilterAsync<TConstructorParameters> _filter;
        private readonly string _parameterName;

        public RequestParameterFilterAsyncOfTRequestFilterAsyncWrapper(IRequestParameterFilterAsync<TConstructorParameters> filter, string parameterName)
        {
            _filter = filter;
            _parameterName = parameterName;
        }

        public Task OnRequestAsync(IRequestEventArgs<TConstructorParameters> eventArgs,
            CancellationToken cancellationToken)
        {
            var value = eventArgs.Parameters[_parameterName];

            return _filter.OnRequestAsync(
                new RequestParameterEventArgs<TConstructorParameters>(eventArgs.ConstructorParameters, _parameterName,
                    value, eventArgs.Request), cancellationToken);
        }
    }

    public class RequestParameterFilterRequestFilterWrapper<TConstructorParameters> : IRequestFilter<TConstructorParameters>
    {
        private readonly IRequestParameterFilter _filter;
        private readonly string _parameterName;

        public RequestParameterFilterRequestFilterWrapper(IRequestParameterFilter filter, string parameterName)
        {
            _filter = filter;
            _parameterName = parameterName;
        }

        public void OnRequest(IRequestEventArgs<TConstructorParameters> eventArgs)
        {
            var value = eventArgs.Parameters[_parameterName];

            _filter.OnRequest(new RequestParameterEventArgs<TConstructorParameters>(eventArgs.ConstructorParameters,
                _parameterName,
                value, eventArgs.Request));
        }
    }
}
