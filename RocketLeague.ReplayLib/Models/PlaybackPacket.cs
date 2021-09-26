using RocketLeague.ReplayLib.Models.Enums;

namespace RocketLeague.ReplayLib.Models
{
    public class PlaybackPacket
    {
        public int DataLength { get; set; }
        public float TimeSeconds { get; set; }
        public int LevelIndex { get; set; }
        public uint SeenLevelIndex { get; set; }
        public PacketState State { get; set; }
    }
}