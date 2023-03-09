using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace buy_sale.host.Converters
{
    public class TimeOnlyConverterJson : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(TimeOnly));
            return TimeOnly.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
