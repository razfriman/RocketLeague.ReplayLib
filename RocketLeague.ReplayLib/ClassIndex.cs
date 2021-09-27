using System.IO;
using RocketLeague.ReplayLib.Extensions;

namespace RocketLeague.ReplayLib
{
    public class ClassIndex
    {
        public string Class { get; private set; }
        public int Index { get; private set; }

        public static ClassIndex Deserialize(BinaryReader bs)
        {
            var classIndex = new ClassIndex
            {
                Class = bs.ReadFString(),
                Index = bs.ReadInt32()
            };
            return classIndex;
        }
    }
}