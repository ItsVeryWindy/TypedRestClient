using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using TypedRestClient.Filters.Requests;

namespace TypedRestClient.Json
{
    public class JsonBodyFilter : IRequestParameterFilter
    {
        private static readonly UTF8Encoding _encoding = new UTF8Encoding(false);
        private readonly JsonSerializer _jsonSerializer;

        public JsonBodyFilter(JsonSerializerSettings settings)
        {
            _jsonSerializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.CreateDefault(settings);
        }

        public void OnRequest(IRequestParameterEventArgs eventArgs)
        {
            var parameter = eventArgs.ParameterValue;

            if (parameter == null)
                throw new ArgumentNullException(eventArgs.ParameterName);

            var ms = new MemoryStream();

            using (var sw = new StreamWriter(ms, _encoding, 1024, true))
            using (var writer = new JsonTextWriter(sw))
                _jsonSerializer.Serialize(writer, parameter);

            ms.Position = 0;

            eventArgs.Request.Content = new StreamContent(ms);
        }
    }
}
