using System;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ClientLoadoutsOnline
    {
        public ClientLoadoutOnline LoadoutOnline1 { get; private set; }
        public ClientLoadoutOnline LoadoutOnline2 { get; private set; }
        public bool Unknown1 { get; private set; }
        public bool Unknown2 { get; private set; }

        public static ClientLoadoutsOnline Deserialize(BitReader br, uint engineVersion, uint licenseeVersion,
            string[] objectNames)
        {
            var clo = new ClientLoadoutsOnline
            {
                LoadoutOnline1 = ClientLoadoutOnline.Deserialize(br, engineVersion, licenseeVersion, objectNames),
                LoadoutOnline2 = ClientLoadoutOnline.Deserialize(br, engineVersion, licenseeVersion, objectNames)
            };

            if (clo.LoadoutOnline1.ProductAttributeLists.Count != clo.LoadoutOnline2.ProductAttributeLists.Count)
            {
                throw new Exception("ClientLoadoutOnline list counts must match");
            }

            clo.Unknown1 = br.ReadBit();
            clo.Unknown2 = br.ReadBit();
            return clo;
        }
    }
}