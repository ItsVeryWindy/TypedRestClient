using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TypedRestClient.Filters.Exceptions
{
    public interface IExceptionEventArgs
    {
        IReadOnlyDictionary<string, object> Parameters { get; }
        HttpRequestMessage Request { get; }
        Exception Exception { get; }
        bool ExceptionHandled { get; set; }
    }
}
