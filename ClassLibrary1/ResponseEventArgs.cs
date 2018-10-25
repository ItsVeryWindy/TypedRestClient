using System.Collections.Generic;
using System.Net.Http;

namespace ClassLibrary1
{
    public interface IResponseEventArgs<TReturn>
    {
        IReadOnlyDictionary<string, object> Parameters { get; }
        HttpResponseMessage Response { get; }
        TReturn ReturnValue { get; set; }
    }

    public interface IResponseEventArgs<out TConstructorParameters, TReturn> : IResponseEventArgs<TReturn>
    {
        TConstructorParameters ConstructorParameters { get; }
    }

    public class ResponseEventArgs<TConstructorParameters, TReturn> : IResponseEventArgs<TConstructorParameters, TReturn>
    {
        public TConstructorParameters ConstructorParameters { get; }
        public IReadOnlyDictionary<string, object> Parameters { get; }
        public HttpResponseMessage Response { get; }
        public TReturn ReturnValue { get; set; }

        public ResponseEventArgs(TConstructorParameters constructorParameters, IReadOnlyDictionary<string, object> parameters, HttpResponseMessage response, TReturn returnValue)
        {
            Parameters = parameters;
            Response = response;
            ReturnValue = returnValue;
            ConstructorParameters = constructorParameters;
        }
    }
}
