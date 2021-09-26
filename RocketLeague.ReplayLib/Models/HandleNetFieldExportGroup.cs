using System.Collections.Generic;
using RocketLeague.ReplayLib.Models.Enums;

namespace RocketLeague.ReplayLib.Models
{
    //Throws unknown handles here instead of throwing warnings 
    public abstract class HandleNetFieldExportGroup : NetFieldExportGroupBase
    {
        public abstract RepLayoutCmdType Type { get; protected set; }
        public Dictionary<uint, object> UnknownHandles = new();
    }
}