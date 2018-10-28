using Microsoft.Extensions.DependencyInjection;
using System;
using TypedRestClient.Filters;

namespace TypedRestClient.DependencyInjection
{
    /// <summary>
    /// Creates a new filter by means of the service container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class FromTypeAttribute : Attribute, IFilterFactory
    {
        private readonly ObjectFactory _factory;

        public FromTypeAttribute(Type type)
        {
            _factory = ActivatorUtilities.CreateFactory(type, new Type[0]);
        }

        public object CreateFilter(TypedRestClientConfiguration configuration)
        {
            var serviceProvider = configuration.Get<DependencyInjectionConfiguration>().ServiceProvider;

            return _factory(serviceProvider, null);
        }
    }
}
