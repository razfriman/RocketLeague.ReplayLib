using System.Collections.Generic;
using System.IO;
using RocketLeague.ReplayLib.Exceptions;
using RocketLeague.ReplayLib.IO;

namespace RocketLeague.ReplayLib.Properties
{
    public class FPropertyParser
    {
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
                case "ByteProperty":
                    var enumName = reader.ReadFString();
                    var enumMember = reader.ReadFString();
                    child.EnumValue = $"{enumName}::{enumMember}";
                    break;
                case "ArrayProperty":
                    var elementCount = reader.ReadInt32();
                    for (var i = 0; i < elementCount; i++)
                    {
                        child.Children.Add(DeserializePropertyContainer(reader));
                    }

                    break;
                default:
                    throw new InvalidReplayException($"Unknown FProperty type: {type}");
            }

            return child;
        }
    }
}