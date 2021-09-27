namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedStatEvent
    {
        public bool Unknown1 { get; private set; }

        // If >= 0, will map to an object like "StatEvents.Events.EpicSave".
        // Finally!
        public int ObjectId { get; private set; }

        public static ReplicatedStatEvent Deserialize(BitReader br)
        {
            var rse = new ReplicatedStatEvent
            {
                Unknown1 = br.ReadBit(),
                ObjectId = br.ReadInt32()
            };
            return rse;
        }
    }
}