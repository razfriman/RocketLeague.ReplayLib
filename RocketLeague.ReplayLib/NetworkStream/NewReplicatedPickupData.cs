namespace RocketLeague.ReplayLib.NetworkStream
{
    public class NewReplicatedPickupData
    {
        public bool Unknown1 { get; private set; }
        public int ActorId { get; private set; }
        public byte Unknown2 { get; private set; }

        public static NewReplicatedPickupData Deserialize(BitReader br)
        {
            var rpd = new NewReplicatedPickupData
            {
                Unknown1 = br.ReadBit(),
                ActorId = br.ReadInt32(),
                Unknown2 = br.ReadByte()
            };

            return rpd;
        }
    }
}