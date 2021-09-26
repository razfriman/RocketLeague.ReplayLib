using RocketLeague.ReplayLib.IO;

namespace RocketLeague.ReplayLib.Models.Properties
{
    public class FName : IProperty
    {
        public string Name { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            Name = reader.SerializePropertyName();
        }

        public override string ToString() => Name;
    }
}