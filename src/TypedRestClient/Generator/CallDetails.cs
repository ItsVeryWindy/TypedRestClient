using System.Collections.Generic;
using TypedRestClient.Filters;

namespace TypedRestClient.Generator
{
    internal class CallDetails
    {
        public List<IFilterFactory> FilterFactories { get; } = new List<IFilterFactory>();
        public Dictionary<string, int> ParameterIndices { get; set; } = new Dictionary<string, int>();
    }
}