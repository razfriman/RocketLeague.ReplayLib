using System.Collections.Generic;
using System.Text;

namespace RocketLeague.ReplayLib.Extensions
{
    public static class BitEnumerableExtensions
    {
        public static string ToBinaryString(this IEnumerable<bool> bits)
        {
            var sb = new StringBuilder();
            foreach (var bit in bits)
            {
                sb.Append(bit ? "1" : "0");
            }

            return sb.ToString();
        }
    }
}