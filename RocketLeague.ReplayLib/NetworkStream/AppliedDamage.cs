namespace RocketLeague.ReplayLib.NetworkStream
{
    public class AppliedDamage
    {
        public byte Unknown1 { get; private set; }
        public Vector3D Position { get; private set; }
        public int Unknown3 { get; private set; }
        public int Unknown4 { get; private set; }

        public static AppliedDamage Deserialize(BitReader br, uint netVersion)
        {
            var ad = new AppliedDamage
            {
                Unknown1 = br.ReadByte(),
                Position = Vector3D.Deserialize(br, netVersion),
                Unknown3 = br.ReadInt32(),
                Unknown4 = br.ReadInt32()
            };
            return ad;
        }
    }
}