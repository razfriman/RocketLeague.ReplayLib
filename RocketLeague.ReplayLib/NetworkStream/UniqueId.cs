using System;
using System.Collections.Generic;
using System.Linq;
using RocketLeague.ReplayLib.Extensions;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class UniqueId
    {
        public enum UniqueIdType
        {
            Unknown = 0,
            Steam = 1,
            Ps4 = 2,
            Ps3 = 3,
            Xbox = 4,
            Switch = 6,
            Psynet = 7,
            Epic = 11
        }

        public UniqueIdType Type { get; protected set; }
        public byte[] Id { get; private set; }
        public byte PlayerNumber { get; private set; } // Split screen player (0 when not split screen)

        public static UniqueId Deserialize(BitReader br, uint licenseeVersion, uint netVersion)
        {
            List<object> data = new();
            var type = (UniqueIdType)br.ReadByte();

            var uid = type switch
            {
                UniqueIdType.Steam => new SteamId(),
                UniqueIdType.Ps4 => new Ps4Id(),
                _ => new UniqueId()
            };
            uid.Type = type;

            DeserializeId(br, uid, licenseeVersion, netVersion);
            return uid;
        }

        protected static void DeserializeId(BitReader br, UniqueId uid, uint licenseeVersion, uint netVersion)
        {
            switch (uid.Type)
            {
                case UniqueIdType.Steam:
                    uid.Id = br.ReadBytes(8);
                    break;
                case UniqueIdType.Ps4 when netVersion >= 1:
                    uid.Id = br.ReadBytes(40);
                    break;
                case UniqueIdType.Ps4:
                    uid.Id = br.ReadBytes(32);
                    break;
                case UniqueIdType.Unknown when licenseeVersion >= 18 && netVersion == 0:
                    return;
                case UniqueIdType.Unknown:
                {
                    uid.Id = br.ReadBytes(3); // Will be 0
                    if (uid.Id.Sum(x => x) != 0 && (licenseeVersion < 18 || netVersion > 0))
                    {
                        throw new Exception("Unknown id isn't 0, might be lost");
                    }

                    break;
                }
                case UniqueIdType.Xbox:
                    uid.Id = br.ReadBytes(8);
                    break;
                case UniqueIdType.Switch:
                    uid.Id = br.ReadBytes(32);
                    break;
                case UniqueIdType.Psynet when netVersion >= 10:
                    uid.Id = br.ReadBytes(8);
                    break;
                case UniqueIdType.Psynet:
                    uid.Id = br.ReadBytes(32);
                    break;
                case UniqueIdType.Epic:
                {
                    // This is really a "GetString", but keeping everything as bytes.
                    var id = br.ReadBytes(4);
                    var len = (id[3] << 24) + (id[2] << 16) + (id[1] << 8) + id[0];
                    uid.Id = id.Concat(br.ReadBytes(len)).ToArray();
                    break;
                }
                default:
                    throw new ArgumentException(
                        $"Invalid type: {((int)uid.Type).ToString()}. Next bits are {br.GetBits(br.Position, Math.Min(4096, br.Length - br.Position)).ToBinaryString()}");
            }

            uid.PlayerNumber = br.ReadByte();
        }

        public override string ToString() => $"{Type} {BitConverter.ToString(Id ?? Array.Empty<byte>()).Replace("-", "")} {PlayerNumber}";
    }

    // Do I really need a separate type for this? 
}