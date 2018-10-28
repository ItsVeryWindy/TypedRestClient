using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TypedRestClient.Json.Tests
{
    public interface IJsonBodyTest
    {
        Task<JObject> GetAsync([JsonBody] object body);
    }
}
