using BenchmarkDotNet.Attributes;

namespace RocketLeague.ReplayLib.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(1, 5, 5)]
    public class BenchmarkReadReplay
    {
        private readonly ReplayReader _replayReader;

        private const string ReplayFile = "/Users/razfriman/Desktop/f635c7e8-51ce-43cb-b43e-cb46a574fc70.replay";

        public BenchmarkReadReplay()
        {
            _replayReader = new ReplayReader();
        }

        [Benchmark]
        public void ParseReplay()
        {
            var replay = _replayReader.Deserialize(ReplayFile);
        }
        
        [Benchmark]
        public void ParseReplayHeader()
        {
            var replay = _replayReader.DeserializeHeader(ReplayFile);
        }
    }
}