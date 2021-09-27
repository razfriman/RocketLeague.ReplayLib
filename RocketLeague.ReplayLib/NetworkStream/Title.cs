namespace RocketLeague.ReplayLib.NetworkStream
{
    public class Title
    {
        public bool Unknown1 { get; private set; }
        public bool Unknown2 { get; private set; }
        public uint Unknown3 { get; private set; }
        public uint Unknown4 { get; private set; }
        public uint Unknown5 { get; private set; }
        public uint Unknown6 { get; private set; }
        public uint Unknown7 { get; private set; }
        public bool Unknown8 { get; private set; }

        public static Title Deserialize(BitReader br)
        {
            // Bit alignment is best guess based on a single example, so could be off.
            var t = new Title
            {
                Unknown1 = br.ReadBit(),
                Unknown2 = br.ReadBit(),
                Unknown3 = br.ReadUInt32(),
                Unknown4 = br.ReadUInt32(),
                Unknown5 = br.ReadUInt32(),
                Unknown6 = br.ReadUInt32(),
                Unknown7 = br.ReadUInt32(),
                Unknown8 = br.ReadBit()
            };

            return t;
        }
    }
}