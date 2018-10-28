using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypedRestClient.Filters.Responses;

namespace TypedRestClient.DependencyInjection.Tests
{
    public class InjectedResponseFilter : IResponseFilter
    {
        private readonly object _injectedInstance;

        public InjectedResponseFilter(object injectedInstance)
        {
            _injectedInstance = injectedInstance;
        }

        public void OnResponse<TReturn>(IResponseEventArgs<TReturn> eventArgs)
        {
            eventArgs.ReturnValue = Task.FromResult((TReturn)_injectedInstance);
        }
    }
}
