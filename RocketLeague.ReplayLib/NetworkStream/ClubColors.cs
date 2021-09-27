namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ClubColors
    {
        public bool Unknown1 { get; private set; }
        public byte Unknown2 { get; private set; }
        public bool Unknown3 { get; private set; }
        public byte Unknown4 { get; private set; }

        public static ClubColors Deserialize(BitReader br)
        {
            var cc = new ClubColors
            {
                Unknown1 = br.ReadBit(),
                Unknown2 = br.ReadByte(),
                Unknown3 = br.ReadBit(),
                Unknown4 = br.ReadByte()
            };
            return cc;
        }
    }
}