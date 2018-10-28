using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TypedRestClient.Shared;

namespace TypedRestClient.Json.Tests
{
    public class JsonBodyAttributeTests : TestBase<IJsonBodyTest>
    {
        [Test]
        public async Task ShouldSerializeAsRequestBody()
        {
            var body = new TestBody
            {
                TestProperty = "TestValue"
            };

            var result = await UnitUnderTest.GetAsync(body);

            Assert.That(HttpMessageHandler.Request?.Content, Is.Not.Null);

            var content = await HttpMessageHandler.Request.Content.ReadAsStringAsync();

            Assert.That(content, Is.EqualTo("{\"TestProperty\":\"TestValue\"}"));
        }

        [Test]
        public void ShouldHandleBadSerialization()
        {
            var body = new ExceptionThrowingTestBody();

            Assert.That(() => UnitUnderTest.GetAsync(body), Throws.InstanceOf<JsonException>());
        }

        private class ExceptionThrowingTestBody
        {
            public string TestProperty => throw new Exception();
        }
    }
}
