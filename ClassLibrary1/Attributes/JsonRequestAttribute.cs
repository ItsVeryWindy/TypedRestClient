using System;
using System.Net.Http;
using ClassLibrary1.Filters;
using Newtonsoft.Json;

namespace ClassLibrary1.Attributes
{
    /// <summary>
    /// Serializes a parameter to JSON for the body of a request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class JsonRequestAttribute : Attribute, IRequestFilter, IValidationFilter
    {
        private readonly string _bodyParameter;

        public JsonRequestAttribute(string bodyParameter)
        {
            _bodyParameter = bodyParameter;
        }

        public void OnRequest(IRequestEventArgs eventArgs)
        {
            var parameter = eventArgs.Parameters[_bodyParameter];

            if (parameter == null)
                throw new ArgumentNullException(_bodyParameter);

            eventArgs.Request.Content = new StringContent(JsonConvert.SerializeObject(parameter));
        }

        public bool Validate(IValidationFilterEventArgs eventArgs)
        {
            return eventArgs.Parameters.ContainsKey(_bodyParameter);
        }
    }
}
