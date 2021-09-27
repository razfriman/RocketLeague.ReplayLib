namespace RocketLeague.ReplayLib.NetworkStream
{
    public class RepStatTitle
    {
        public bool Unknown1 { get; private set; }
        public string Name { get; private set; }
        public ObjectTarget ObjectTarget { get; private set; }
        public uint Value { get; private set; }

        public static RepStatTitle Deserialize(BitReader br)
        {
            var rst = new RepStatTitle
            {
                Unknown1 = br.ReadBit(),
                Name = br.ReadString(),
                ObjectTarget = ObjectTarget.Deserialize(br),
                Value = br.ReadUInt32()
            };

            return rst;
        }
    }
}