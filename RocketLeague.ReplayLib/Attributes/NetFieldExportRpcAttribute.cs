using System;

namespace RocketLeague.ReplayLib.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class NetFieldExportRpcAttribute : Attribute
    {
        public string PathName { get; private set; }

        public NetFieldExportRpcAttribute(string typePathname) => PathName = typePathname;
    }
}