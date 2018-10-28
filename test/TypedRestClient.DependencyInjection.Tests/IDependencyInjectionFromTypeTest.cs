using System.Threading.Tasks;

namespace TypedRestClient.DependencyInjection.Tests
{
    public interface IDependencyInjectionFromTypeTest
    {
        [FromType(typeof(InjectedResponseFilter))]
        Task<object> GetAsync();
    }
}
