using System;
using TypedRestClient.Filters.Requests;

namespace TypedRestClient.Filters
{
    internal class RequestParameterFilterFactory : IFilterFactory
    {
        private readonly IFilterFactory _factory;
        private readonly string _parameterName;

        public RequestParameterFilterFactory(IFilterFactory factory, string parameterName)
        {
            _factory = factory;
            _parameterName = parameterName;
        }

        public object CreateFilter(TypedRestClientConfiguration configuration)
        {
            var filter = _factory.CreateFilter(configuration);

            if (filter is IRequestParameterFilter requestFilter)
            {
                return new RequestParameterFilterRequestFilterAsyncWrapper(requestFilter, _parameterName);
            }

            if (filter is IRequestParameterFilterAsync requestFilterAsync)
            {
                return new RequestParameterFilterAsyncRequestFilterAsyncWrapper(requestFilterAsync, _parameterName);
            }

            throw new InvalidOperationException("Invalid Request Parameter Filter");
        }
    }
}
