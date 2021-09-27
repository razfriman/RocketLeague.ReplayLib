using System;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ProductAttribute
    {
        public bool Unknown1 { get; private set; }
        public uint ClassIndex { get; private set; }
        public string ClassName { get; private set; }

        public bool HasValue { get; private set; } // Only used for UserColor_TA

        // Would rather this be strongly typed, but this will work for now
        public object Value { get; private set; }

        private const int
            MaxValue = 14; // This may need tweaking, but it works well enough for now. Only used in older replays

        public static ProductAttribute Deserialize(BitReader br, uint engineVersion, uint licenseeVersion,
            string[] objectNames)
        {
            var pa = new ProductAttribute
            {
                Unknown1 = br.ReadBit(),
                ClassIndex = br.ReadUInt32()
            };

            pa.ClassName = objectNames[pa.ClassIndex];

            switch (pa.ClassName)
            {
                case "TAGame.ProductAttribute_UserColor_TA" when licenseeVersion >= 23:
                    pa.Value = new[]
                    {
                        br.ReadByte(),
                        br.ReadByte(),
                        br.ReadByte(),
                        br.ReadByte()
                    };
                    break;
                case "TAGame.ProductAttribute_UserColor_TA":
                {
                    if (pa.HasValue = br.ReadBit())
                    {
                        pa.Value = br.ReadUInt32FromBits(31);
                    }

                    break;
                }
                case "TAGame.ProductAttribute_Painted_TA" when engineVersion >= 868 && licenseeVersion >= 18:
                    pa.Value = br.ReadUInt32FromBits(31);
                    break;
                case "TAGame.ProductAttribute_Painted_TA":
                    pa.Value = br.ReadUInt32Max(MaxValue);
                    break;
                case "TAGame.ProductAttribute_TitleID_TA":
                    pa.Value = br.ReadString();
                    break;
                case "TAGame.ProductAttribute_SpecialEdition_TA":
                case "TAGame.ProductAttribute_TeamEdition_TA":
                    pa.Value = br.ReadUInt32FromBits(31);
                    break;
                // I've never encountered this attribute, but Psyonix_Cone mentioned it serialized as below. Leaving it commented out until I can test it.
                default:
                    throw new Exception("Unknown product attribute class " + pa.ClassName);
            }

            return pa;
        }
    }
}