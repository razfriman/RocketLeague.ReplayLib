namespace RocketLeague.ReplayLib.NetworkStream
{
    public class WeldedInfo
    {
        public bool Active { get; private set; }
        public int ActorId { get; private set; }
        public Vector3D Offset { get; private set; }
        public float Mass { get; private set; }
        public Rotator Rotation { get; private set; }

        public static WeldedInfo Deserialize(BitReader br, uint netVersion)
        {
            var wi = new WeldedInfo
            {
                Active = br.ReadBit(),
                ActorId = br.ReadInt32(),
                Offset = Vector3D.Deserialize(br, netVersion),
                Mass = br.ReadFloat(),
                Rotation = Rotator.Deserialize(br)
            };

            return wi;
        }
    }
}