namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ClientLoadout
    {
        public byte Version { get; private set; } // Always 10, except when it's 11

        // Product Ids are in TAGame.upk in the ProductsDB content

        public uint BodyProductId { get; private set; }
        public uint SkinProductId { get; private set; }
        public uint WheelProductId { get; private set; }
        public uint BoostProductId { get; private set; }
        public uint AntennaProductId { get; private set; }
        public uint HatProductId { get; private set; }

        public uint Unknown2 { get; private set; } // Always 0. Future expansion room for a different product type?

        public uint Unknown3 { get; private set; }

        public uint EngineAudioProductId { get; private set; }
        public uint TrailProductId { get; private set; }
        public uint GoalExplosionProductId { get; private set; }

        public uint
            BannerProductId
        {
            get;
            private set;
        } // I didn't check if this is actually the banner id, but it's the only new customization I know of.

        public uint Unknown4 { get; private set; }
        public uint Unknown5 { get; private set; }
        public uint Unknown6 { get; private set; }
        public uint Unknown7 { get; private set; }

        public static ClientLoadout Deserialize(BitReader br)
        {
            var cl = new ClientLoadout
            {
                Version = br.ReadByte(),
                BodyProductId = br.ReadUInt32(),
                SkinProductId = br.ReadUInt32(),
                WheelProductId = br.ReadUInt32(),
                BoostProductId = br.ReadUInt32(),
                AntennaProductId = br.ReadUInt32(),
                HatProductId = br.ReadUInt32(),
                Unknown2 = br.ReadUInt32()
            };

            if (cl.Version > 10)
            {
                cl.Unknown3 = br.ReadUInt32();
            }

            if (cl.Version >= 16)
            {
                cl.EngineAudioProductId = br.ReadUInt32();
                cl.TrailProductId = br.ReadUInt32();
                cl.GoalExplosionProductId = br.ReadUInt32();
            }

            if (cl.Version >= 17)
            {
                cl.BannerProductId = br.ReadUInt32();
            }

            if (cl.Version >= 19)
            {
                cl.Unknown4 = br.ReadUInt32();
            }

            if (cl.Version >= 22)
            {
                cl.Unknown5 = br.ReadUInt32();
                cl.Unknown6 = br.ReadUInt32();
                cl.Unknown7 = br.ReadUInt32();
            }

            return cl;
        }

        public override string ToString()
        {
            return
                $"Version {Version}, BodyProductId {BodyProductId}, SkinProductId {SkinProductId}, WheelProductId {WheelProductId}, BoostProductId {BoostProductId}, AntennaProductId {AntennaProductId}, HatProductId {HatProductId}, Unknown2 {Unknown2}";
        }
    }
}