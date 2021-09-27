namespace RocketLeague.ReplayLib.NetworkStream
{
    public class TeamPaint
    {
        public byte TeamNumber { get; private set; }

        // Almost definitely the BlueTeam/OrangeTeam colors from CarColors in TAGame.upk
        public byte TeamColorId { get; private set; }

        // Almost definitely the Accent colors from CarColors in TAGame.upk
        public byte CustomColorId { get; private set; }

        // Finish Ids are in TAGame.upk in the ProductsDB content
        public uint TeamFinishId { get; private set; }
        public uint CustomFinishId { get; private set; }

        public static TeamPaint Deserialize(BitReader br)
        {
            var tp = new TeamPaint
            {
                TeamNumber = br.ReadByte(),
                TeamColorId = br.ReadByte(),
                CustomColorId = br.ReadByte(),
                TeamFinishId = br.ReadUInt32(),
                CustomFinishId = br.ReadUInt32()
            };

            return tp;
        }

        public override string ToString()
        {
            return
                $"TeamNumber:{TeamNumber}, TeamColorId:{TeamColorId}, TeamFinishId:{TeamFinishId}, CustomColorId:{CustomColorId}, CustomFinishId:{CustomFinishId}";
        }
    }
}