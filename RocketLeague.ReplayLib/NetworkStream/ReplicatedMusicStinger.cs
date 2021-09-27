namespace RocketLeague.ReplayLib.NetworkStream
{
    public sealed class ReplicatedMusicStinger
    {
        public bool Unknown1 { get; private set; }

        public uint ObjectIndex { get; private set; }

        // Seems to start at 2 and increases by 1 every time it shows up.
        public byte Unknown2 { get; private set; }

        public static ReplicatedMusicStinger Deserialize(BitReader br)
        {
            var rms = new ReplicatedMusicStinger
            {
                Unknown1 = br.ReadBit(),
                ObjectIndex = br.ReadUInt32(),
                Unknown2 = br.ReadByte()
            };

            return rms;
        }
    }
}