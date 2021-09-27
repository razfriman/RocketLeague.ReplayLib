namespace RocketLeague.ReplayLib.NetworkStream
{
    public class RigidBodyState
    {
        public bool Sleeping { get; private set; }
        public IVector3D Position { get; private set; }

        // I dont like this as an object, but it'll do for now.
        // Can be Vector3D or Quaternion
        public object Rotation { get; private set; }

        public Vector3D LinearVelocity { get; private set; }
        public Vector3D AngularVelocity { get; private set; }

        public static RigidBodyState Deserialize(BitReader br, uint netVersion)
        {
            var rbs = new RigidBodyState
            {
                Sleeping = br.ReadBit()
            };
            if (netVersion >= 5)
            {
                rbs.Position = FixedPointVector3D.Deserialize(br, netVersion);
            }
            else
            {
                rbs.Position = Vector3D.Deserialize(br, netVersion);
            }

            if (netVersion >= 7)
            {
                rbs.Rotation = Quaternion.Deserialize(br);
            }
            else
            {
                rbs.Rotation = Vector3D.DeserializeFixed(br);
            }

            if (!rbs.Sleeping)
            {
                rbs.LinearVelocity = Vector3D.Deserialize(br, netVersion);
                rbs.AngularVelocity = Vector3D.Deserialize(br, netVersion);
            }

            return rbs;
        }

        public override string ToString() =>
            Sleeping 
                ? $"Position: {Position} Rotation {Rotation}"
                : $"Position: {Position} Rotation {Rotation} LinearVelocity {LinearVelocity} AngularVelocity {AngularVelocity}";
    }
}