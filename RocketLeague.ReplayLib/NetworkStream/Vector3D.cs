namespace RocketLeague.ReplayLib.NetworkStream
{
    public class Vector3D : IVector3D
    {
        private uint NumBits { get; set; }
        private uint Dx { get; set; }
        private uint Dy { get; set; }
        private uint Dz { get; set; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public static Vector3D Deserialize(BitReader br, uint netVersion)
        {
            return Deserialize(netVersion >= 7 ? 22 : 20, br);
        }

        private Vector3D()
        {
        }

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private static Vector3D Deserialize(int maxBits, BitReader br)
        {
            var v = new Vector3D
            {
                // Simplified from ReadPackedVector
                NumBits = br.ReadUInt32Max(maxBits)
            };

            var bias = 1 << (int)(v.NumBits + 1);
            var max = (int)v.NumBits + 2;

            v.Dx = br.ReadUInt32FromBits(max);
            v.Dy = br.ReadUInt32FromBits(max);
            v.Dz = br.ReadUInt32FromBits(max);

            v.X = v.Dx - bias;
            v.Y = v.Dy - bias;
            v.Z = v.Dz - bias;

            return v;
        }

        public static Vector3D DeserializeFixed(BitReader br)
        {
            var v = new Vector3D
            {
                X = br.ReadFixedCompressedFloat(1, 16),
                Y = br.ReadFixedCompressedFloat(1, 16),
                Z = br.ReadFixedCompressedFloat(1, 16)
            };

            return v;
        }

        public override string ToString() => $"(X:{X}, Y:{Y}, Z:{Z})";
    }
}