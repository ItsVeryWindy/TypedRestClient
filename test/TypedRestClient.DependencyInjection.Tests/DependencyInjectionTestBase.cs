using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TypedRestClient.Shared;

namespace TypedRestClient.DependencyInjection.Tests
{
    public class DependencyInjectionTestBase<T> : TestBase<T>
    {
        protected object InjectedInstance { get; private set; }

        public override void Configure(TypedRestClientConfiguration configuration)
        {
            base.Configure(configuration);

            InjectedInstance = new object();

            configuration.AddServiceProvider(
                new ServiceCollection()
                .AddSingleton(InjectedInstance)
                .AddSingleton<InjectedResponseFilter>()
                .BuildServiceProvider());
        }
    }
}
