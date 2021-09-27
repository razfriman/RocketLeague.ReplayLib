namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedPickupData
    {
        public bool Unknown1 { get; private set; }
        public int ActorId { get; private set; }
        public bool Unknown2 { get; private set; }

        public static ReplicatedPickupData Deserialize(BitReader br)
        {
            var rpd = new ReplicatedPickupData
            {
                Unknown1 = br.ReadBit(),
                ActorId = br.ReadInt32(),
                Unknown2 = br.ReadBit()
            };

            return rpd;
        }
    }
}