namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedDemolishCustomFx
    {
        public bool Unknown1 { get; private set; }
        public int Unknown2 { get; private set; }
        public ReplicatedDemolish ReplicatedDemolish { get; private set; }

        public static ReplicatedDemolishCustomFx Deserialize(BitReader br, uint netVersion)
        {
            var rd = new ReplicatedDemolishCustomFx
            {
                Unknown1 = br.ReadBit(),
                Unknown2 = br.ReadInt32(),
                ReplicatedDemolish = ReplicatedDemolish.Deserialize(br, netVersion)
            };

            return rd;
        }
    }
}