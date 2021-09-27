using System;
using System.IO;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class SteamId : UniqueId
    {
        public long SteamId64
        {
            get
            {
                if (Type != UniqueIdType.Steam)
                {
                    throw new InvalidDataException($"Invalid type {Type}, cant extract steam id");
                }

                return BitConverter.ToInt64(Id, 0);
            }
        }

        public string SteamProfileUrl => $"http://steamcommunity.com/profiles/{SteamId64}";

        public override string ToString()
        {
            return $"{SteamId64} ({SteamProfileUrl}), PlayerNumber {PlayerNumber}";
        }
    }
}