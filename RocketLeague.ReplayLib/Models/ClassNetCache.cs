using System.Collections.Generic;

namespace RocketLeague.ReplayLib.Models
{
    public class ClassNetCache
    {
        public int ObjectIndex { get; set; }
        public int ParentId { get; set; }
        public int ClassNetCacheId { get; set; }
        public List<ClassNetCacheProperty> Properties { get; set; } = new();
    }
}