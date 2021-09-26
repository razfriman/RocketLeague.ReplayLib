using System;
using System.IO;
using RocketLeague.ReplayLib.IO;
using Xunit;

namespace RocketLeague.ReplayLib.Test
{
    public class UnrealBinaryReaderTest
    {
        [Fact]
        public unsafe void Test()
        {
            var n = 1024;
            var data = new byte[n];
            for (var i = 0; i < n; i++)
            {
                data[i] = (byte)i;
            }
            using var ms = new MemoryStream(data);
            using var reader1 = new UnrealBinaryReader(ms);
            var netReader1 = new NetBitReader();
            var netReader2 = new BitReader();
            var buffer = reader1.GetMemoryBuffer(n);
            netReader1.SetBits(buffer.PositionPointer, n, n * 8);
            netReader2.SetBits(buffer.PositionPointer, n, n * 8);

            reader1.Seek(0);
            var a1 = reader1.ReadInt32();
            var a2 = reader1.ReadInt32();

            var b1 = netReader1.ReadInt32();
            var b2 = netReader1.ReadInt32();

            var c1 = netReader2.ReadInt32();
            var c2 = netReader2.ReadInt32();

            Console.WriteLine("Done");
        }
    }
}