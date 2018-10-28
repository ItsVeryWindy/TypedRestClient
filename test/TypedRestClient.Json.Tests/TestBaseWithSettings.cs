using Newtonsoft.Json;
using System;
using TypedRestClient.Shared;

namespace TypedRestClient.Json.Tests
{
    public class TestBaseWithSettings<T> : TestBase<T>
    {
        public override void Configure(TypedRestClientConfiguration configuration)
        {
            base.Configure(configuration);

            configuration.AddJsonSerializerSettings(new JsonSerializerSettings
            {
                Converters =
                    {
                        new StubJsonConverter()
                    }
            });
        }

        private class StubJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(TestBody);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new TestBody
                {
                    TestProperty = "OveriddenValue"
                };
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, new
                {
                    TestProperty = "OveriddenValue"
                });
            }
        }
    }
}
