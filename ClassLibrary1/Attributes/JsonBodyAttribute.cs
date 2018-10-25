using System;
using System.Linq;
using System.Net.Http;
using ClassLibrary1.Filters;
using Newtonsoft.Json;

namespace ClassLibrary1.Attributes
{
    /// <summary>
    /// Serializes a parameter to JSON for the body of a request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class JsonBodyAttribute : Attribute, IRequestParameterFilter, IValidationFilter
    {
        public void OnRequest(IRequestParameterEventArgs eventArgs)
        {
            var parameter = eventArgs.ParameterValue;

            if (parameter == null)
                throw new ArgumentNullException(eventArgs.ParameterName);

            eventArgs.Request.Content = new StringContent(JsonConvert.SerializeObject(parameter));
        }

        public bool Validate(IValidationFilterEventArgs eventArgs)
        {
            var parameterCount = eventArgs.Parameters.Values
                .Count(v => v.Attributes.Any(a => a.GetType() == typeof(JsonBodyAttribute)));

            return parameterCount == 1;
        }
    }
}
