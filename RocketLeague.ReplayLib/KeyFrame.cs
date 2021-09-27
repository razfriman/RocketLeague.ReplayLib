using System.IO;

namespace RocketLeague.ReplayLib
{
    public class KeyFrame
    {
        public float Time { get; private set; }
        public int Frame { get; private set; }
        public int FilePosition { get; private set; }

        public static KeyFrame Deserialize(BinaryReader bs)
        {
            var keyFrame = new KeyFrame
            {
                Time = bs.ReadSingle(),
                Frame = bs.ReadInt32(),
                FilePosition = bs.ReadInt32()
            };
            return keyFrame;
        }
    }
}