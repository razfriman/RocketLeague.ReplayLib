namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedDemolish
    {
        public bool Unknown1 { get; private set; }
        public int AttackerActorId { get; private set; }
        public bool Unknown2 { get; private set; }
        public uint VictimActorId { get; private set; } // Always equals this actor's id

        public Vector3D
            AttackerVelocity { get; private set; } // Not verified. Attacker/Victim velocity could be swapped

        public Vector3D VictimVelocity { get; private set; }

        public static ReplicatedDemolish Deserialize(BitReader br, uint netVersion)
        {
            var rd = new ReplicatedDemolish
            {
                Unknown1 = br.ReadBit(),
                AttackerActorId = br.ReadInt32(),
                Unknown2 = br.ReadBit(),
                VictimActorId = br.ReadUInt32(),
                AttackerVelocity = Vector3D.Deserialize(br, netVersion),
                VictimVelocity = Vector3D.Deserialize(br, netVersion)
            };

            return rd;
        }
    }
}