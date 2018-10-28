using System.IO;
using System.Threading.Tasks;
using TypedRestClient.Attributes;

namespace TypedRestClient.Tests.Attributes
{
    public interface IStreamResponseTest
    {
        [StreamResponse]
        Task<Stream> Get();
    }
}
