using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TypedRestClient.Filters.Exceptions;
using TypedRestClient.Filters.Requests;
using TypedRestClient.Filters.Responses;

namespace TypedRestClient.Generator
{
    internal class HttpClientProxy : IHttpClientProxy
    {
        private readonly List<CallDetails> _callDetails;

        public HttpClientProxy(List<CallDetails> callDetails)
        {
            _callDetails = callDetails;
        }

        public List<Filters> Initialize(HttpClient client, TypedRestClientConfiguration configuration)
        {
            var filtersList = new List<Filters>();

            foreach(var callDetails in _callDetails)
            {
                var filters = new Filters();

                filtersList.Add(filters);

                foreach(var factory in callDetails.FilterFactories)
                {
                    var filter = factory.CreateFilter(configuration);

                    if (filter is IRequestFilter requestFilter)
                    {
                        filters.RequestFilters.Add(new RequestFilterWrapperAsync(requestFilter));
                    }

                    if (filter is IRequestFilterAsync requestFilterAsync)
                    {
                        filters.RequestFilters.Add(requestFilterAsync);
                    }

                    if (filter is IResponseFilter responseFilter)
                    {
                        filters.ResponseFilters.Add(new ResponseFilterWrapperAsync(responseFilter));
                    }

                    if (filter is IResponseFilterAsync responseFilterAsync)
                    {
                        filters.ResponseFilters.Add(responseFilterAsync);
                    }

                    if (filter is IExceptionFilter exceptionFilter)
                    {
                        filters.ExceptionFilters.Add(new ExceptionFilterWrapperAsync(exceptionFilter));
                    }

                    if (filter is IExceptionFilterAsync exceptionFilterAsync)
                    {
                        filters.ExceptionFilters.Add(exceptionFilterAsync);
                    }
                }
            }

            return filtersList;
        }

        public async Task<TReturn> CallClient<TReturn>(HttpClient httpClient, List<Filters> filtersList, int methodId, object[] parameters, CancellationToken cancellationToken)
        {
            var details = _callDetails[methodId];
            var filters = filtersList[methodId];

            var parameterDictionary = new ParameterDictionary(parameters, details.ParameterIndices);

            var request = new HttpRequestMessage();

            var requestEventArgs = new RequestEventArgs(parameterDictionary, request);

            foreach (var filter in filters.RequestFilters)
            {
                await filter.OnRequestAsync(requestEventArgs, cancellationToken);
            }

            HttpResponseMessage response = null;

            try
            {
                response = await httpClient.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                var exceptionEventArgs = new ExceptionEventArgs(parameterDictionary, request, ex);

                foreach (var filter in filters.ExceptionFilters)
                {
                    await filter.OnExceptionAsync(exceptionEventArgs, cancellationToken);
                }

                if (!exceptionEventArgs.ExceptionHandled)
                    throw;
            }

            var responseEventArgs = new ResponseEventArgs<TReturn>(parameterDictionary, response, Task.FromResult(default(TReturn)));

            foreach (var resAttr in filters.ResponseFilters)
            {
                await resAttr.OnResponseAsync(responseEventArgs, cancellationToken);
            }

            return await responseEventArgs.ReturnValue;
        }
    }
}
