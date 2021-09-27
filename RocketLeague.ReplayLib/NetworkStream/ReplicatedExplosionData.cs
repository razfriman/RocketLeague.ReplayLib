namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedExplosionData
    {
        public bool Unknown1 { get; private set; }
        public uint ActorId { get; private set; } // Probably
        public Vector3D Position { get; private set; }

        public static ReplicatedExplosionData Deserialize(BitReader br, uint netVersion)
        {
            var red = new ReplicatedExplosionData();

            red.DeserializeImpl(br, netVersion);

            return red;
        }

        protected virtual void DeserializeImpl(BitReader br, uint netVersion)
        {
            Unknown1 = br.ReadBit();
            ActorId = br.ReadUInt32();
            Position = Vector3D.Deserialize(br, netVersion);
        }
    }
}