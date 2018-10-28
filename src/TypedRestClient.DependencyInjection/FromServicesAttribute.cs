using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TypedRestClient.Filters;

namespace TypedRestClient.DependencyInjection
{
    /// <summary>
    /// Resolves a filter from the service container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class FromServicesAttribute : Attribute, IFilterFactory
    {
        private readonly Type _type;

        public FromServicesAttribute(Type type)
        {
            _type = type;
        }

        public object CreateFilter(TypedRestClientConfiguration configuration)
        {
            var serviceProvider = configuration.Get<DependencyInjectionConfiguration>().ServiceProvider;

            return serviceProvider.GetRequiredService(_type);
        }
    }
}
