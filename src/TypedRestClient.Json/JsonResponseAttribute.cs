using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TypedRestClient.Filters;
using TypedRestClient.Filters.Responses;

namespace TypedRestClient.Json
{
    /// <summary>
    /// Deserializes the body of the response from JSON into the return type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class JsonResponseAttribute : Attribute, IFilterFactory
    {
        public object CreateFilter(TypedRestClientConfiguration configuration)
        {
            var settings = configuration.Get<JsonSerializerSettingsConfiguration>().SerializerSettings;

            return new JsonResponseFilter(settings);
        }
    }
}
