namespace RocketLeague.ReplayLib.NetworkStream
{
    // Not sure about this name...
    public class FixedPointVector3D : IVector3D
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        private Vector3D _integerVector;

        public static FixedPointVector3D Deserialize(BitReader br, uint netVersion)
        {
            var vector = new FixedPointVector3D
            {
                _integerVector = Vector3D.Deserialize(br, netVersion)
            };
            // Could make this generic for any number of decimal places, but this is all I need for now
            vector.X = vector._integerVector.X / 100;
            vector.Y = vector._integerVector.Y / 100;
            vector.Z = vector._integerVector.Z / 100;

            return vector;
        }

        public override string ToString() => $"(X:{X}, Y:{Y}, Z:{Z})";
    }
}