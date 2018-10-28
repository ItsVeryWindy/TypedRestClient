using System;
using System.Threading;
using System.Threading.Tasks;
using TypedRestClient.Filters.Responses;
using TypedRestClient.Filters.Validation;

namespace TypedRestClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class StringResponseAttribute : Attribute, IResponseFilter, IValidationFilter
    {
        public void OnResponse<TReturn>(IResponseEventArgs<TReturn> eventArgs)
        {
            var content = eventArgs.Response.Content;

            if (content == null)
                return;

            eventArgs.ReturnValue = (Task<TReturn>)(object)content.ReadAsStringAsync();
        }

        public bool Validate(IValidationFilterEventArgs eventArgs)
        {
            return eventArgs.ReturnType == typeof(string);
        }
    }
}
