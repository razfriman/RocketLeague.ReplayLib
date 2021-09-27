using System.IO;
using System.Text;

namespace RocketLeague.ReplayLib.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadFString(this BinaryReader br)
        {
            var length = br.ReadInt32();
            switch (length)
            {
                case > 0:
                {
                    var bytes = br.ReadBytes(length);

                    return Encoding.ASCII.GetString(bytes, 0, length - 1);
                }
                case < 0:
                {
                    var bytes = br.ReadBytes(length * -2);
                    return Encoding.Unicode.GetString(bytes, 0, length * -2 - 2);
                }
                default:
                    return "";
            }
        }
    }
}