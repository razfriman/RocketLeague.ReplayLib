namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ReplicatedDemolishGoalExplosion
    {
        public bool GoalExplosionOwnerFlag { get; private set; }
        public int GoalExplosionOwner { get; private set; }
        public bool AttackerFlag { get; private set; }
        public int AttackerActorId { get; private set; }
        public bool VictimFlag { get; private set; }
        public uint VictimActorId { get; private set; }
        public Vector3D AttackerVelocity { get; private set; }
        public Vector3D VictimVelocity { get; private set; }

        public static ReplicatedDemolishGoalExplosion Deserialize(BitReader br, uint netVersion)
        {
            var rd = new ReplicatedDemolishGoalExplosion
            {
                GoalExplosionOwnerFlag = br.ReadBit(),
                GoalExplosionOwner = br.ReadInt32(),
                AttackerFlag = br.ReadBit(),
                AttackerActorId = br.ReadInt32(),
                VictimFlag = br.ReadBit(),
                VictimActorId = br.ReadUInt32(),
                AttackerVelocity = Vector3D.Deserialize(br, netVersion),
                VictimVelocity = Vector3D.Deserialize(br, netVersion)
            };

            return rd;
        }
    }
}