using NUnit.Framework;
using System;
using TypedRestClient.Generator;

namespace TypedRestClient.Json.Tests
{
    public class JsonBodyAttributeValidationTests
    {
        [Test]
        public void ShouldOnlyAllowOneJsonBodyTag()
        {
            Assert.That(() => TypedRestClientGenerator.CreateFactory<IJsonBodyDuplicateTest>(), Throws.TypeOf<TypeLoadException>());
        }
    }
}
