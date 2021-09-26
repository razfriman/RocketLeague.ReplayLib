using System.Collections.Generic;

namespace RocketLeague.ReplayLib.Models
{
    public class Replay
    {
        public uint EngineVersion { get; set; }
        public uint LicenseeVersion { get; set; }
        public uint NetVersion { get; set; }
        public string ReplayClass { get; set; } = "TAGame.Replay_Soccar_TA";
        public FProperty PropertyContainer { get; set; } = new();
        public List<Level> Levels { get; set; } = new();
        public List<KeyFrame> KeyFrames { get; set; } = new();
        public List<Frame> Frames { get; set; } = new();
        public List<DebugString> DebugStrings { get; set; } = new();
        public List<TickMark> TickMarks { get; set; } = new();
        public List<string> Packages { get; set; } = new();
        public List<string> Objects { get; set; } = new();
        public List<string> Names { get; set; } = new();
        public List<ClassIndex> ClassIndexes { get; set; } = new();
        public List<ClassNetCache> ClassNetCaches { get; set; } = new();
        public long ParseTime { get; set; }
    }
}