using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TypedRestClient.Json.Tests
{
    public interface IJsonBodyDuplicateTest
    {
        Task<JObject> GetAsync([JsonBody] object body, [JsonBody] object alsoBody);
    }
}
