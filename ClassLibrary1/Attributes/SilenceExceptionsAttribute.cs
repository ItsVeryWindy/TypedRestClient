using System;
using ClassLibrary1.Filters;

namespace ClassLibrary1.Attributes
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
