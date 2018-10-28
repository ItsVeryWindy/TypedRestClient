using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace TypedRestClient.Json.Tests
{
    public class JsonResponseAttributeWithSettingsTests : TestBaseWithSettings<IJsonResponseTest>
    {
        [Test]
        public async Task ShouldDeserializeUsingSettings()
        {
            HttpMessageHandler.Response = new HttpResponseMessage
            {
                Content = new StringContent("{\"TestProperty\": \"TestValue\"}")
            };

            var result = await UnitUnderTest.Get();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TestProperty, Is.EqualTo("OveriddenValue"));
        }
    }
}
