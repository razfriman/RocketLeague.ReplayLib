using System.IO;
using RocketLeague.ReplayLib.Extensions;

namespace RocketLeague.ReplayLib
{
    public class DebugString
    {
        public int FrameNumber { get; private set; }
        public string Username { get; private set; }
        public string Text { get; private set; }

        public static DebugString Deserialize(BinaryReader br)
        {
            var ds = new DebugString
            {
                FrameNumber = br.ReadInt32(),
                Username = br.ReadFString(),
                Text = br.ReadFString()
            };
            return ds;
        }

        public override string ToString() => $"{FrameNumber} {Username} {Text}";
    }
}