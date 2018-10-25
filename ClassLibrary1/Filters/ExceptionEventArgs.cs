using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ClassLibrary1.Filters
{
    public interface IExceptionEventArgs
    {
        IReadOnlyDictionary<string, object> Parameters { get; }
        HttpRequestMessage Request { get; }
        Exception Exception { get; }
        bool ExceptionHandled { get; set; }
    }

    public interface IExceptionEventArgs<out TConstructorParameters> : IExceptionEventArgs
    {
        TConstructorParameters ConstructorParameters { get; }
    }

    public class ExceptionEventArgs<TConstructorParameters> : IExceptionEventArgs<TConstructorParameters>
    {
        public TConstructorParameters ConstructorParameters { get; }
        public IReadOnlyDictionary<string, object> Parameters { get; }
        public HttpRequestMessage Request { get; }
        public Exception Exception { get; }
        public bool ExceptionHandled { get; set; }

        public ExceptionEventArgs(TConstructorParameters constructorParameters, IReadOnlyDictionary<string, object> parameters, HttpRequestMessage request, Exception exception)
        {
            Parameters = parameters;
            Request = request;
            ConstructorParameters = constructorParameters;
            Exception = exception;
        }
    }
}
