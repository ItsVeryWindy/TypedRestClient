using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TypedRestClient.Filters.Exceptions
{
    internal class ExceptionEventArgs : IExceptionEventArgs
    {
        public IReadOnlyDictionary<string, object> Parameters { get; }
        public HttpRequestMessage Request { get; }
        public Exception Exception { get; }
        public bool ExceptionHandled { get; set; }

        public ExceptionEventArgs(IReadOnlyDictionary<string, object> parameters, HttpRequestMessage request, Exception exception)
        {
            Parameters = parameters;
            Request = request;
            Exception = exception;
        }
    }
}
