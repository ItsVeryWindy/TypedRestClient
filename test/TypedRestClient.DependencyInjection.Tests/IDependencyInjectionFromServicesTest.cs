using System.Threading.Tasks;

namespace TypedRestClient.DependencyInjection.Tests
{
    public interface IDependencyInjectionFromServicesTest
    {
        [FromServices(typeof(InjectedResponseFilter))]
        Task<object> GetAsync();
    }
}
