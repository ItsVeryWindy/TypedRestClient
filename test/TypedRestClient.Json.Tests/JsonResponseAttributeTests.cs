using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using TypedRestClient.Shared;

namespace TypedRestClient.Json.Tests
{
    public class JsonResponseAttributeTests : TestBase<IJsonResponseTest>
    {
        [Test]
        public async Task ShouldDeserializeResponseBody()
        {
            HttpMessageHandler.Response = new HttpResponseMessage
            {
                Content = new StringContent("{\"TestProperty\": \"TestValue\"}")
            };

            var result = await UnitUnderTest.Get();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TestProperty, Is.EqualTo("TestValue"));
        }

        [Test]
        public void ShouldHandleBadSerialization()
        {
            HttpMessageHandler.Response = new HttpResponseMessage
            {
                Content = new StringContent("{\"not-good-content\"}")
            };

            Assert.That(() => UnitUnderTest.Get(), Throws.InstanceOf<JsonException>());
        }
    }
}
