using System.IO;
using RocketLeague.ReplayLib.Extensions;

namespace RocketLeague.ReplayLib
{
    public class Level
    {
        public string Name { get; private set; }

        public static Level Deserialize(BinaryReader bs)
        {
            var level = new Level
            {
                Name = bs.ReadFString()
            };
            return level;
        }
    }
}