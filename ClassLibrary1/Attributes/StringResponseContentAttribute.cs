using System;
using System.Collections.Generic;
using ClassLibrary1.Filters;

namespace ClassLibrary1.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class StringResponseContentAttribute : Attribute, IResponseFilter, IValidationFilter
    {
        public void OnResponse<TReturn>(IResponseEventArgs<TReturn> eventArgs)
        {
            eventArgs.ReturnValue = (TReturn)(object)"hello";
        }

        public bool Validate(IValidationFilterEventArgs eventArgs)
        {
            return eventArgs.ReturnType == typeof(string);
        }
    }
}
