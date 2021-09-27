using System.IO;
using RocketLeague.ReplayLib.Extensions;

namespace RocketLeague.ReplayLib
{
    public class TickMark
    {
        public string Type { get; private set; }
        public int Frame { get; private set; } // Frame?

        public static TickMark Deserialize(BinaryReader bs)
        {
            var tm = new TickMark
            {
                Type = bs.ReadFString(),
                Frame = bs.ReadInt32()
            };
            return tm;
        }
    }
}