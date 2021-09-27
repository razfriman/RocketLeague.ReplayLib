using System.IO;

namespace RocketLeague.ReplayLib
{
    public class ClassNetCacheProperty
    {
        public int Index { get; private set; }
        public int Id { get; private set; }

        public static ClassNetCacheProperty Deserialize(BinaryReader br)
        {
            var prop = new ClassNetCacheProperty
            {
                Index = br.ReadInt32(),
                Id = br.ReadInt32()
            };
            return prop;
        }
    }
}