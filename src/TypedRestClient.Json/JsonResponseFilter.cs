using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypedRestClient.Filters.Responses;

namespace TypedRestClient.Json
{
    internal class JsonResponseFilter : IResponseFilterAsync
    {
        private static readonly UTF8Encoding _encoding = new UTF8Encoding(false);
        private readonly JsonSerializer _jsonSerializer;

        public JsonResponseFilter(JsonSerializerSettings settings)
        {
            _jsonSerializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.CreateDefault(settings);
        }

        public async Task OnResponseAsync<TReturn>(IResponseEventArgs<TReturn> eventArgs, CancellationToken cancellationToken)
        {
            using (var sr = new StreamReader(await eventArgs.Response.Content.ReadAsStreamAsync()))
            using (var reader = new JsonTextReader(sr))
                eventArgs.ReturnValue = Task.FromResult(_jsonSerializer.Deserialize<TReturn>(reader));
        }
    }
}
