using System;
using System.Reflection;
using RocketLeague.ReplayLib.Attributes;
using RocketLeague.ReplayLib.Models;

namespace RocketLeague.ReplayLib.NetFields
{
    public sealed class NetFieldInfo
    {
        public NetFieldExportAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public object DefaultValue { get; set; }
        public int ElementTypeId { get; set; }
        public Action<NetFieldExportGroupBase, object> SetMethod { get; set; }
    }
}