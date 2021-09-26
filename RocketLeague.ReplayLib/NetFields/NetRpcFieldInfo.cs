using System.Reflection;
using RocketLeague.ReplayLib.Attributes;

namespace RocketLeague.ReplayLib.NetFields
{
    public sealed class NetRpcFieldInfo
    {
        public NetFieldExportRpcPropertyAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public bool IsCustomStructure { get; set; }
    }
}