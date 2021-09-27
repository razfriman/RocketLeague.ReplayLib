namespace RocketLeague.ReplayLib.NetworkStream
{
    public class PickupInfo
    {
        // This breakdown of the bits is a wild guess, obviously
        public bool Unknown1;
        public bool Unknown2;
        public uint Unknown3; // ActorId?
        public int Unknown4;
        public int Unknown5;
        public bool Unknown6;
        public bool Unknown7;

        public static PickupInfo Deserialize(BitReader br)
        {
            return new PickupInfo
            {
                Unknown1 = br.ReadBit(),
                Unknown2 = br.ReadBit(),
                Unknown3 = br.ReadUInt32(),
                Unknown4 = br.ReadInt32(),
                Unknown5 = br.ReadInt32(),
                Unknown6 = br.ReadBit(),
                Unknown7 = br.ReadBit()
            };
        }
    }
}