using NUnit.Framework;
using System.Threading.Tasks;

namespace TypedRestClient.DependencyInjection.Tests
{
    public class FromTypeAttributeTests : DependencyInjectionTestBase<IDependencyInjectionFromTypeTest>
    {
        [Test]
        public async Task ShouldInjectInstance()
        {
            var result = await UnitUnderTest.GetAsync();

            Assert.That(result, Is.EqualTo(InjectedInstance));
        }
    }
}
