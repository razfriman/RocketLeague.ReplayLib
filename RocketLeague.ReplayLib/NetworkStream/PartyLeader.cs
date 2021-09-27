using System.Collections.Generic;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class PartyLeader : UniqueId
    {
        public new static PartyLeader Deserialize(BitReader br, uint licenseeVersion, uint netVersion)
        {
            PartyLeader pl = new();

            List<object> data = new();
            pl.Type = (UniqueIdType)br.ReadByte();

            if (pl.Type != UniqueIdType.Unknown)
            {
                DeserializeId(br, pl, licenseeVersion, netVersion);
            }

            return pl;
        }
    }
}