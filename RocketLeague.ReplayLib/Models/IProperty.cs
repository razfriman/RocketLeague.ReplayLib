using RocketLeague.ReplayLib.IO;

namespace RocketLeague.ReplayLib.Models
{
    public interface IProperty
    {
        void Serialize(NetBitReader reader);
    }
}