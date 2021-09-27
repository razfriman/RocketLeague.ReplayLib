using System.Collections.Generic;
using System.Linq;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class PrivateMatchSettings
    {
        public IEnumerable<string> Mutators { get; private set; }
        public uint Unknown1 { get; private set; }
        public uint Unknown2 { get; private set; }
        public string GameName { get; private set; }
        public string Password { get; private set; }
        public bool Unknown3 { get; private set; }

        public static PrivateMatchSettings Deserialize(BitReader br)
        {
            var pms = new PrivateMatchSettings
            {
                Mutators = br.ReadString().Split(',').ToList(),
                Unknown1 = br.ReadUInt32(), // GameNameId? Possibly referencing a string by id
                Unknown2 = br.ReadUInt32(), // Max players?
                GameName = br.ReadString(),
                Password = br.ReadString(),
                Unknown3 = br.ReadBit() // Public?
            };

            return pms;
        }

        public override string ToString() => $"Mutators: [{string.Join(", ", Mutators)}], Unknown1 {Unknown1}, Unknown2 {Unknown2}, GameName {GameName}, Password {Password}, Unknown3 {Unknown3}";
    }
}