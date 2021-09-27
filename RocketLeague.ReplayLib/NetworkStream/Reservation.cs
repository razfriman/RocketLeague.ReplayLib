namespace RocketLeague.ReplayLib.NetworkStream
{
    public class Reservation
    {
        public uint Unknown1 { get; private set; }
        public UniqueId PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public byte Unknown2 { get; private set; }


        public static Reservation Deserialize(uint engineVersion, uint licenseeVersion, uint netVersion, BitReader br)
        {
            var r = new Reservation
            {
                Unknown1 = br.ReadUInt32FromBits(3),
                PlayerId = UniqueId.Deserialize(br, licenseeVersion, netVersion)
            };

            if (r.PlayerId.Type != UniqueId.UniqueIdType.Unknown)
            {
                r.PlayerName = br.ReadString();
            }

            if (engineVersion < 868 || licenseeVersion < 12)
            {
                r.Unknown2 = br.ReadBitsAsBytes(2)[0];
            }
            else
            {
                r.Unknown2 = br.ReadByte();
            }
            /*
                ReservationStatus_None,
    ReservationStatus_Reserved,
    ReservationStatus_Joining,
    ReservationStatus_InGame,
    ReservationStatus_MAX
             */

            return r;
        }

        public override string ToString()
        {
            // TODO: Since the 2 versions are Reservation arent 2 different classes, this doesnt know which version to output
            // Make separate classes, or store the version (yuck)?
            return $"Unknown1: {Unknown1} ID: {PlayerId} Name: {PlayerName} Unknown2: {Unknown2}";
        }
    }
}