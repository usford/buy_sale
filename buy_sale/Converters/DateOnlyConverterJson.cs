﻿using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace buy_sale.host.Converters
{
    public class DateOnlyConverterJson : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateOnly));
            return DateOnly.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
