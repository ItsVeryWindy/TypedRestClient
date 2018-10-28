using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TypedRestClient.Shared;

namespace TypedRestClient.Tests.Attributes
{
    public class StreamResponseAttributeTests : TestBase<IStreamResponseTest>
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

            Assert.That(new StreamReader(result).ReadToEnd(), Is.EqualTo(expectedString));
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
