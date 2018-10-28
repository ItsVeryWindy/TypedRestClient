using System;
using TypedRestClient.Filters.Exceptions;

namespace TypedRestClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SilenceExceptionsAttribute : Attribute, IExceptionFilter
    {
        public void OnException(IExceptionEventArgs eventArgs)
        {
            eventArgs.ExceptionHandled = true;
        }
    }
}
