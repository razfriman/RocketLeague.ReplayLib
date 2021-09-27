namespace RocketLeague.ReplayLib.NetworkStream
{
    public class Rotator
    {
        public float Pitch { get; private set; }
        public float Yaw { get; private set; }
        public float Roll { get; private set; }

        private static float Normalize(float axis)
        {
            axis %= 360f;
            if (axis < 0) axis += 360;
            return axis;
        }

        private static byte AxisToByte(float axis)
        {
            return (byte)(Normalize(axis) / 360f * 256f);
        }

        private static float ByteToAxis(byte b)
        {
            return b / 256f * 360f;
        }

        public static Rotator Deserialize(BitReader br)
        {
            var r = new Rotator();
            if (br.ReadBit())
            {
                r.Pitch = ByteToAxis(br.ReadByte());
            }

            if (br.ReadBit())
            {
                r.Yaw = ByteToAxis(br.ReadByte());
            }

            if (br.ReadBit())
            {
                r.Roll = ByteToAxis(br.ReadByte());
            }

            return r;
        }
    }
}