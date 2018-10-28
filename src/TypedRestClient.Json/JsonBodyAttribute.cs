using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using TypedRestClient.Filters;
using TypedRestClient.Filters.Requests;
using TypedRestClient.Filters.Validation;

namespace TypedRestClient.Json
{
    /// <summary>
    /// Serializes a parameter to JSON for the body of a request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class JsonBodyAttribute : Attribute, IFilterFactory, IValidationFilter
    {
        public object CreateFilter(TypedRestClientConfiguration configuration)
        {
            var settings = configuration.Get<JsonSerializerSettingsConfiguration>().SerializerSettings;

            return new JsonBodyFilter(settings);
        }

        public bool Validate(IValidationFilterEventArgs eventArgs)
        {
            var parameterCount = eventArgs.Parameters.Values
                .Count(v => v.Attributes.Any(a => a.GetType() == typeof(JsonBodyAttribute)));

            return parameterCount == 1;
        }
    }
}
