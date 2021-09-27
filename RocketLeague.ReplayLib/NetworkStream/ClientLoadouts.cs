namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ClientLoadouts
    {
        public ClientLoadout Loadout1 { get; private set; } // Blue or orange?
        public ClientLoadout Loadout2 { get; private set; }

        public static ClientLoadouts Deserialize(BitReader br)
        {
            var clo = new ClientLoadouts
            {
                Loadout1 = ClientLoadout.Deserialize(br),
                Loadout2 = ClientLoadout.Deserialize(br)
            };
            return clo;
        }
    }
}