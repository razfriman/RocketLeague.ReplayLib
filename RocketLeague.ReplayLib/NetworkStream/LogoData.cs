namespace RocketLeague.ReplayLib.NetworkStream
{
    public class LogoData
    {
        public bool Unknown1 { get; private set; } // Betting this indicates team 0/1 or orange/blue palette? 
        public uint LogoId { get; private set; } // Bit of a guess, havent gone fishing in the UPKs for matches

        public static LogoData Deserialize(BitReader br) =>
            new()
            {
                Unknown1 = br.ReadBit(),
                LogoId = br.ReadUInt32()
            };

        public override string ToString() => $"Unknown1: {Unknown1}, LogoId: {LogoId}";
    }
}