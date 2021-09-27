using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RocketLeague.ReplayLib
{
    [JsonConverter(typeof(FPropertyJsonConverter))]
    public sealed class FProperty
    {
        public string Name { get; init; }
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
    }
}