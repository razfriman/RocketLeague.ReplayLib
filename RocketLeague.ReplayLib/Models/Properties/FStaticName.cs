using RocketLeague.ReplayLib.IO;
using RocketLeague.ReplayLib.Models.Enums;

namespace RocketLeague.ReplayLib.Models.Properties
{
    public class FStaticName : IProperty
    {
        public string Value { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            var isHardcoded = reader.ReadBoolean();

            if (isHardcoded)
            {
                uint nameIndex;
            nameIndex = reader.ReadPackedUInt32();

                Value = UnrealNameConstants.Names[nameIndex];
                return;
            }

            var inString = reader.ReadFString();
            var inNumber = reader.ReadInt32();

            Value = inString;
        }

        public override string ToString() => Value;
    }
}