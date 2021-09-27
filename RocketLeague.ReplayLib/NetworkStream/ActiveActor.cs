namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ActiveActor
    {
        public bool Active { get; private set; }
        public int ActorId { get; private set; }

        public static ActiveActor Deserialize(BitReader br)
        {
            // If Active == false, ActorId will be -1
            var aa = new ActiveActor
            {
                Active = br.ReadBit(),
                ActorId = br.ReadInt32()
            };
            return aa;
        }

        public override string ToString() => $"Active: {Active}, ActorId: {ActorId}";
    }
}