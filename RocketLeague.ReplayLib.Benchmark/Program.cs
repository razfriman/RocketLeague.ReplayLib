using BenchmarkDotNet.Running;

namespace RocketLeague.ReplayLib.Benchmark
{
    /*
|            Method |          Mean |        Error |       StdDev |     Gen 0 |     Gen 1 |    Gen 2 | Allocated |
|------------------ |--------------:|-------------:|-------------:|----------:|----------:|---------:|----------:|
|       ParseReplay | 116,145.10 us | 5,997.464 us | 1,557.522 us | 5000.0000 | 2200.0000 | 800.0000 | 28,196 KB |
| ParseReplayHeader |      70.75 us |     1.048 us |     0.272 us |    9.8877 |    4.8828 |   0.1221 |     61 KB |

     */
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<BenchmarkReadReplay>();
        }
    }
}