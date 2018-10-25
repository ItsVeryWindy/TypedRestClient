using System.Net.Http;

namespace ClassLibrary1.Filters
{
    public interface IRequestParameterEventArgs
    {
        string ParameterName { get; }
        object ParameterValue { get; }
        HttpRequestMessage Request { get; }
    }

    public interface IRequestParameterEventArgs<out TConstructorParameters> : IRequestParameterEventArgs
    {
        TConstructorParameters ConstructorParameters { get; }
    }

    public class RequestParameterEventArgs<TConstructorParameters> : IRequestParameterEventArgs<TConstructorParameters>
    {
        public TConstructorParameters ConstructorParameters { get; }
        public string ParameterName { get; }
        public object ParameterValue { get; }
        public HttpRequestMessage Request { get; }

        public RequestParameterEventArgs(TConstructorParameters constructorParameters, string parameterName, object parameterValue, HttpRequestMessage request)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
            Request = request;
            ConstructorParameters = constructorParameters;
        }
    }
}
