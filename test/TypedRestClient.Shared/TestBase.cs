using NUnit.Framework;
using System;
using System.Net.Http;
using TypedRestClient.Generator;

namespace TypedRestClient.Shared
{
    public class TestBase<T>
    {
        ITypedRestClientFactory<T> _factory;
        protected T UnitUnderTest { get; private set; }
        protected StubHttpMessageHandler HttpMessageHandler { get; private set; }

        [OneTimeSetUp]
        public void TestBaseOneTimeSetUp()
        {
            var factoryConfiguration = new FactoryConfiguration()
                .AddFilter(new DefaultRouteFilter());

            _factory = TypedRestClientGenerator.CreateFactory<T>(factoryConfiguration);
        }

        [SetUp]
        public void TestBaseSetUp()
        {
            HttpMessageHandler = new StubHttpMessageHandler();

            var client = new HttpClient(HttpMessageHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var configuration = new TypedRestClientConfiguration();

            Configure(configuration);

            UnitUnderTest = _factory.Create(client, configuration);
        }

        public virtual void Configure(TypedRestClientConfiguration configuration)
        {

        }
    }
}
