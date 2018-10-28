using System.Threading.Tasks;

namespace TypedRestClient.Json.Tests
{
    public interface IJsonResponseTest
    {
        [JsonResponse]
        Task<TestBody> Get();
    }
}
