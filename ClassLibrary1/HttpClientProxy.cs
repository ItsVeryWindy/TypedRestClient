using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1.Filters;

namespace ClassLibrary1
{
    internal class HttpClientProxy<TConstructorParameters> : IHttpClientProxy<TConstructorParameters>
    {
        private readonly List<CallDetails<TConstructorParameters>> _callDetails;

        public HttpClientProxy(List<CallDetails<TConstructorParameters>> callDetails)
        {
            _callDetails = callDetails;
        }

        public async Task<TReturn> CallClient<TReturn>(HttpClient httpClient, TConstructorParameters constructorParameters, int methodId, object[] parameters, CancellationToken cancellationToken)
        {
            var details = _callDetails[methodId];

            var parameterDictionary = new ParameterDictionary(parameters, details.ParameterIndices);

            var request = new HttpRequestMessage();

            var requestEventArgs = new RequestEventArgs<TConstructorParameters>(constructorParameters, parameterDictionary, request);

            foreach (var filter in details.RequestFilters)
            {
                await filter.OnRequestAsync(requestEventArgs, cancellationToken);
            }

            try
            {

            }
            catch (Exception ex)
            {
                var exceptionEventArgs = new ExceptionEventArgs<TConstructorParameters>(constructorParameters, parameterDictionary, request, ex);

                foreach (var filter in details.ExceptionFilters)
                {
                    await filter.OnExceptionAsync(exceptionEventArgs, cancellationToken);
                }

                if (!exceptionEventArgs.ExceptionHandled)
                    throw;
            }

            var response = new HttpResponseMessage
            {
                Content = new StringContent("\"hello\"")
            };

            var responseEventArgs = new ResponseEventArgs<TConstructorParameters, TReturn>(constructorParameters, parameterDictionary, response, default(TReturn));

            foreach (var resAttr in details.ResponseFilters)
            {
                await resAttr.OnResponseAsync(responseEventArgs, cancellationToken);
            }

            return responseEventArgs.ReturnValue;
        }
    }
}
