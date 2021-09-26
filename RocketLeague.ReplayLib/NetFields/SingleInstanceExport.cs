using RocketLeague.ReplayLib.Extensions;
using RocketLeague.ReplayLib.Models;

namespace RocketLeague.ReplayLib.NetFields
{
    public sealed class SingleInstanceExport
    {
        internal NetFieldExportGroupBase Instance { get; set; }
        internal FastClearArray<NetFieldInfo> ChangedProperties { get; set; }
    }
}