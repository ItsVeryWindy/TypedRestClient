using NUnit.Framework;
using System.Threading.Tasks;

namespace TypedRestClient.Json.Tests
{
    public class JsonBodyAttributeWithSettingsTests : TestBaseWithSettings<IJsonBodyTest>
    {
        [Test]
        public async Task ShouldSerializeUsingSettings()
        {
            var body = new TestBody
            {
                TestProperty = "TestValue"
            };

            var result = await UnitUnderTest.GetAsync(body);

            Assert.That(HttpMessageHandler.Request?.Content, Is.Not.Null);

            var content = await HttpMessageHandler.Request.Content.ReadAsStringAsync();

            Assert.That(content, Is.EqualTo("{\"TestProperty\":\"OveriddenValue\"}"));
        }
    }
}
