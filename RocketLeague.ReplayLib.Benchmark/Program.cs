using BenchmarkDotNet.Running;

namespace RocketLeague.ReplayLib.Benchmark
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<BenchmarkReadReplay>();
        }
    }
}