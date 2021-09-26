using System.IO;
using BenchmarkDotNet.Attributes;
using RocketLeague.ReplayLib.IO;

namespace RocketLeague.ReplayLib.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(1, 3, 3)]
    public class BenchmarkReadReplay
    {
        private readonly RocketLeagueReplayReader _replayReader;

        private const string ReplayFile = "/Users/razfriman/Desktop/f635c7e8-51ce-43cb-b43e-cb46a574fc70.replay";

        public BenchmarkReadReplay()
        {
            _replayReader = new RocketLeagueReplayReader(null);
        }

        [Benchmark]
        public unsafe void ParseReplay()
        {
            // var replay = _replayReader.ReadReplay(ReplayFile);
            var n = 100000;
            var data = new byte[n];
            for (var i = 0; i < n; i++)
            {
                data[i] = (byte)i;
            }
            using var ms = new MemoryStream(data);
            using var reader1 = new UnrealBinaryReader(ms);
            using var netReader1 = new NetBitReader();
            using var netReader2 = new BitReader();
            using var buffer = reader1.GetMemoryBuffer(n);
            using var buffer2= reader1.GetMemoryBuffer(n);
            using var buffer3 = reader1.GetMemoryBuffer(n);
            using var buffer4 = reader1.GetMemoryBuffer(n);
            netReader1.SetBits(buffer.PositionPointer, n, n * 8);
            netReader2.SetBits(buffer.PositionPointer, n, n * 8);

            reader1.Seek(0);
            var a1 = reader1.ReadInt32();
            var a2 = reader1.ReadInt32();

            var b1 = netReader1.ReadInt32();
            var b2 = netReader1.ReadInt32();

            var c1 = netReader2.ReadInt32();
            var c2 = netReader2.ReadInt32();
        }
        
        
    }
}