using System.Threading.Tasks;
using TypedRestClient.Attributes;

namespace TypedRestClient.Tests.Attributes
{
    public interface IStringResponseTest
    {
        [StringResponse]
        Task<string> Get();
    }
}
