using System.Collections.Generic;
using System.Net.Http;

namespace ClassLibrary1.Filters
{
    public interface IRequestEventArgs
    {
        IReadOnlyDictionary<string, object> Parameters { get; }
        HttpRequestMessage Request { get; }
    }

    public interface IRequestEventArgs<out TConstructorParameters> : IRequestEventArgs
    {
        TConstructorParameters ConstructorParameters { get; }
    }

    public class RequestEventArgs<TConstructorParameters> : IRequestEventArgs<TConstructorParameters>
    {
        public TConstructorParameters ConstructorParameters { get; }
        public IReadOnlyDictionary<string, object> Parameters { get; }
        public HttpRequestMessage Request { get; }

        public RequestEventArgs(TConstructorParameters constructorParameters, IReadOnlyDictionary<string, object> parameters, HttpRequestMessage request)
        {
            Parameters = parameters;
            Request = request;
            ConstructorParameters = constructorParameters;
        }
    }
}
