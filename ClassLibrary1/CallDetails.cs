using System.Collections.Generic;
using ClassLibrary1.Filters;

namespace ClassLibrary1
{
    internal class CallDetails<TConstructorParameters>
    {
        public List<IRequestFilterAsync<TConstructorParameters>> RequestFilters { get; } = new List<IRequestFilterAsync<TConstructorParameters>>();
        public List<IResponseFilterAsync<TConstructorParameters>> ResponseFilters { get; } = new List<IResponseFilterAsync<TConstructorParameters>>();
        public List<IExceptionFilterAsync<TConstructorParameters>> ExceptionFilters { get; } = new List<IExceptionFilterAsync<TConstructorParameters>>();
        public Dictionary<string, int> ParameterIndices { get; set; } = new Dictionary<string, int>();
    }
}