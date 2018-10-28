using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using TypedRestClient.Shared;
using TypedRestClient.Tests.Attributes;

namespace TypedRestClient.Tests
{
    public class StringResponseAttributeTests : TestBase<IStringResponseTest>
    {
        [Test]
        public async Task ShouldDeserializeResponseBody()
        {
            const string expectedString = "this is my string";

            HttpMessageHandler.Response = new HttpResponseMessage
            {
                Content = new StringContent(expectedString)
            };

            var result = await UnitUnderTest.Get();

            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public async Task ShouldHandleNoContent()
        {
            HttpMessageHandler.Response = new HttpResponseMessage
            {
                Content = null
            };

            var result = await UnitUnderTest.Get();

            Assert.That(result, Is.Null);
        }
    }
}
