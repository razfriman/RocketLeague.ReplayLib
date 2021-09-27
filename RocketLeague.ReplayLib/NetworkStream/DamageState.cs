namespace RocketLeague.ReplayLib.NetworkStream
{
    public class DamageState
    {
        public byte Unknown1 { get; private set; }
        public bool Unknown2 { get; private set; }
        public int Unknown3 { get; private set; }
        public Vector3D Unknown4 { get; private set; } // Position or Force maybe?
        public bool Unknown5 { get; private set; }
        public bool Unknown6 { get; private set; }

        public static DamageState Deserialize(BitReader br, uint netVersion)
        {
            var ds = new DamageState
            {
                Unknown1 = br.ReadByte(),
                Unknown2 = br.ReadBit(),
                Unknown3 = br.ReadInt32(),
                Unknown4 = Vector3D.Deserialize(br, netVersion),
                Unknown5 = br.ReadBit(),
                Unknown6 = br.ReadBit()
            };
            return ds;
        }
    }
}