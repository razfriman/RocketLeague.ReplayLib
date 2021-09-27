using System;

namespace RocketLeague.ReplayLib.NetworkStream
{
    // https://github.com/jjbott/RocketLeague.ReplayLib/issues/30#issuecomment-410515375
    public class Quaternion
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public float W { get; private set; }

        private const int NumBits = 18;
        private const float MaxQuatValue = 0.7071067811865475244f; // 1/sqrt(2)
        private const float InvMaxQuatValue = 1.0f / MaxQuatValue;

        private enum Component
        {
            X,
            Y,
            Z,
            W
        }

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }


        private static float UncompressComponent(uint iValue)
        {
            const int maxValue = (1 << NumBits) - 1;
            var positiveRangedValue = iValue / (float)maxValue;
            var rangedValue = (positiveRangedValue - 0.50f) * 2.0f;
            return rangedValue * MaxQuatValue;
        }

        private static uint CompressComponent(float value)
        {
            const int maxValue = (1 << NumBits) - 1;
            var rangedValue = value / MaxQuatValue;
            var positiveRangedValue = rangedValue / 2f + .5f;
            return (uint)Math.Round(maxValue * positiveRangedValue);
        }

        public static Quaternion Deserialize(BitReader br)
        {
            var largestComponent = (Component)br.ReadInt32FromBits(2);

            var a = UncompressComponent(br.ReadUInt32FromBits(NumBits));
            var b = UncompressComponent(br.ReadUInt32FromBits(NumBits));
            var c = UncompressComponent(br.ReadUInt32FromBits(NumBits));
            var missing = (float)Math.Sqrt(1.0f - a * a - b * b - c * c);

            switch (largestComponent)
            {
                case Component.X:
                    return new Quaternion(missing, a, b, c);
                case Component.Y:
                    return new Quaternion(a, missing, b, c);
                case Component.Z:
                    return new Quaternion(a, b, missing, c);
                case Component.W:
                    return new Quaternion(a, b, c, missing);
                default:
                    return new Quaternion(a, b, c, missing);
            }
        }

        public override string ToString()
        {
            return $"(X:{X}, Y:{Y}, Z:{Z}), W:{W}";
        }
    }
}