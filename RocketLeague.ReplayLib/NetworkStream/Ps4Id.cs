using System;
using System.Text;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class Ps4Id : UniqueId
    {
        public string PsnName => Encoding.ASCII.GetString(Id, 0, 16).Replace("\0", "");

        public string Unknown1 =>
            // "unknown stuff used internally by ps4 api" - Psyonix_Cone
            Encoding.ASCII.GetString(Id, 16, 8).Replace("\0", "");

        public ulong Unknown2 =>
            // "more unknown stuff" - Psyonix_Cone
            BitConverter.ToUInt64(Id, 24);

        public ulong? PsnId
        {
            get
            {
                if (Id.Length > 32)
                {
                    return BitConverter.ToUInt64(Id, 32);
                }

                return null;
            }
        }

        public override string ToString()
        {
            return $"{PsnName} ({PsnId?.ToString("X") ?? ""}), PlayerNumber {PlayerNumber}";
        }
    }
}