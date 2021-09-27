namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedExplosionDataExtended : ReplicatedExplosionData
    {
        public bool Unknown3 { get; private set; }
        public uint Unknown4 { get; private set; }

        public new static ReplicatedExplosionDataExtended Deserialize(BitReader br, uint netVersion)
        {
            var rede = new ReplicatedExplosionDataExtended();

            rede.DeserializeImpl(br, netVersion);

            return rede;
        }

        protected override void DeserializeImpl(BitReader br, uint netVersion)
        {
            base.DeserializeImpl(br, netVersion);
            Unknown3 = br.ReadBit();
            Unknown4 = br.ReadUInt32();
        }
    }
}