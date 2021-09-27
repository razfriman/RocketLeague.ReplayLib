using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RocketLeague.ReplayLib
{
    public class FPropertyJsonConverter : JsonConverter<FProperty>
    {
        public override void Write(
            Utf8JsonWriter writer,
            FProperty property,
            JsonSerializerOptions options)
        {
            if (property.Properties.Count > 0)
            {
                writer.WriteStartObject();
                foreach (var child in property.Properties.Values)
                {
                    Write(writer, child, options);
                }

                writer.WriteEndObject();
            }
            else if (property.Children.Count > 0)
            {
                writer.WritePropertyName(property.Name);
                writer.WriteStartArray();
                foreach (var child in property.Children)
                {
                    Write(writer, child, options);
                }

                writer.WriteEndArray();
            }
            else if (property.IntValue.HasValue)
            {
                writer.WriteNumber(property.Name, property.IntValue.Value);
            }
            else if (property.FloatValue.HasValue)
            {
                writer.WriteNumber(property.Name, property.FloatValue.Value);
            }
            else if (property.BoolValue.HasValue)
            {
                writer.WriteBoolean(property.Name, property.BoolValue.Value);
            }
            else if (property.QWordValue.HasValue)
            {
                writer.WriteNumber(property.Name, property.QWordValue.Value);
            }
            else if (property.EnumValue != null)
            {
                writer.WriteString(property.Name, property.EnumValue);
            }
            else if (property.StrValue != null)
            {
                writer.WriteString(property.Name, property.StrValue);
            }
            else if (property.NameValue != null)
            {
                writer.WriteString(property.Name, property.NameValue);
            }
        }

        public override FProperty Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            throw new NotImplementedException();
    }
}