using RocketLeague.ReplayLib.IO;

namespace RocketLeague.ReplayLib.Models
{
    public class ExternalData
    {
        public uint NetGuid { get; internal set; }
        public float TimeSeconds { get; internal set; }
        public byte[] Data { get; private set; }

        public virtual void Serialize(FArchive reader, int numBytes)
        {
            Data = reader.ReadBytes(numBytes);
        }
    }
}