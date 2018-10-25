using System.Net.Http;
using System.Threading.Tasks;
using ClassLibrary1.Attributes;
using NUnit.Framework;

namespace ClassLibrary1
{
    public partial class Class1
    {
        [Test]
        public void test()
        {
            var typeFactory = new HttpClientProxyTypeFactory();

            var factory = typeFactory.CreateFactory<ITest>();

            var instance = factory.Create(new HttpClient(), null);

            var response = instance.GetResponseAsync("hello", "body").Result;

            var response2 = instance.GetOtherResponseAsync("hello", "body").Result;

        }

        [Test]
        public void test2()
        {
            var builder = new HttpClientBuilderOptions<(string one, string two), string>()
                .Get(parameters => $"{parameters.one}/{parameters.two}")
                .JsonResponse()
                .Build();

            var response = builder(("hello", "world"));
        }

        [JsonResponse]
        [SilenceExceptions]
        public interface ITest
        {
            [HttpRoute("hello/{input}")]
            Task<string> GetResponseAsync(string input, [JsonBody] string body);

            [HttpRoute("hello/{input}/other")]
            [JsonRequest("test")]
            Task<string> GetOtherResponseAsync(string input, string test);
        }
    }
}
