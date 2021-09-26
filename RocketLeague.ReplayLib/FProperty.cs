using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using RocketLeague.ReplayLib.IO;

namespace RocketLeague.ReplayLib
{
    public sealed class FProperty
    {
        public string Name { get; set; }
        public bool? BoolValue { get; set; }
        public string EnumValue { get; set; }
        public int? IntValue { get; set; }
        public float? FloatValue { get; set; }
        public ulong? QWordValue { get; set; }
        public string StrValue { get; set; }
        public string NameValue { get; set; }
        public Dictionary<string, FProperty> Properties { get; init; } = new();
        public List<FProperty> Children { get; } = new();

        public FProperty this[string property] => Properties[property];

        public string ToJson()
        {
            JsonWriterOptions writerOptions = new() { Indented = true, };
            using MemoryStream stream = new();
            using Utf8JsonWriter writer = new(stream, writerOptions);
            WriteJson(writer);
            writer.Flush();
            var json = Encoding.UTF8.GetString(stream.ToArray());
            return json;
        }

        public void WriteJson(Utf8JsonWriter writer)
        {
            if (Properties.Count > 0)
            {
                writer.WriteStartObject();
                foreach (var child in Properties.Values)
                {
                    child.WriteJson(writer);
                }

                writer.WriteEndObject();
            }
            else if (Children.Count > 0)
            {
                writer.WritePropertyName(Name);
                writer.WriteStartArray();
                foreach (var child in Children)
                {
                    child.WriteJson(writer);
                }

                writer.WriteEndArray();
            }
            else if (IntValue.HasValue)
            {
                writer.WriteNumber(Name, IntValue.Value);
            }
            else if (FloatValue.HasValue)
            {
                writer.WriteNumber(Name, FloatValue.Value);
            }
            else if (BoolValue.HasValue)
            {
                writer.WriteBoolean(Name, BoolValue.Value);
            }
            else if (QWordValue.HasValue)
            {
                writer.WriteNumber(Name, QWordValue.Value);
            }
            else if (EnumValue != null)
            {
                writer.WriteString(Name, EnumValue);
            }
            else if (StrValue != null)
            {
                writer.WriteString(Name, StrValue);
            }
            else if (NameValue != null)
            {
                writer.WriteString(Name, NameValue);
            }
        }

        public static FProperty DeserializePropertyContainer(UnrealBinaryReader reader) =>
            new()
            {
                Properties = DeserializeProperties(reader)
            };

        public static Dictionary<string, FProperty> DeserializeProperties(UnrealBinaryReader reader)
        {
            Dictionary<string, FProperty> properties = new();
            while (true)
            {
                var child = Deserialize(reader);
                if (child is null)
                {
                    break;
                }

                properties.Add(child.Name, child);
            }

            return properties;
        }

        public static FProperty Deserialize(UnrealBinaryReader reader)
        {
            var child = new FProperty
            {
                Name = reader.ReadFString()
            };
            if (child.Name is null or "None")
            {
                return null;
            }

            var type = reader.ReadFString();
            var size = reader.ReadInt32();
            var arrayIndex = reader.ReadInt32();
            switch (type)
            {
                case "IntProperty":
                    child.IntValue = reader.ReadInt32();
                    break;
                case "FloatProperty":
                    child.FloatValue = reader.ReadSingle();
                    break;
                case "ByteProperty":
                    var enumName = reader.ReadFString();
                    var enumMember = reader.ReadFString();
                    child.EnumValue = $"{enumName}::{enumMember}";
                    break;
                case "BoolProperty":
                    child.BoolValue = reader.ReadBoolean();
                    break;
                case "QWordProperty":
                    child.QWordValue = reader.ReadUInt64();
                    break;
                case "StrProperty":
                    child.StrValue = reader.ReadFString();
                    break;
                case "NameProperty":
                    child.NameValue = reader.ReadFString();
                    break;
                case "ArrayProperty":
                    var elementCount = reader.ReadInt32();
                    for (var i = 0; i < elementCount; i++)
                    {
                        child.Children.Add(DeserializePropertyContainer(reader));
                    }

                    break;
                default:
                    Console.WriteLine("Unknown property: " + type);
                    break;
            }

            return child;
        }
    }
}