namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ObjectTarget // I can't think of a better name for whatever this is...
    {
        public bool Unknown1 { get; private set; }
        public int ObjectIndex { get; private set; }

        public static ObjectTarget Deserialize(BitReader br)
        {
            var aa = new ObjectTarget
            {
                Unknown1 = br.ReadBit(),
                ObjectIndex = br.ReadInt32()
            };
            return aa;
        }

        public override string ToString()
        {
            return $"Unknown1: {Unknown1}, ObjectId: {ObjectIndex}";
        }
    }
}