namespace RocketLeague.ReplayLib.NetworkStream
{
    public class CameraSettings
    {
        public float FieldOfView { get; private set; }
        public float Height { get; private set; }
        public float Pitch { get; private set; }
        public float Distance { get; private set; }
        public float Stiffness { get; private set; }
        public float SwivelSpeed { get; private set; }
        public float TransitionSpeed { get; private set; }

        public static CameraSettings Deserialize(BitReader br, uint engineVersion, uint licenseeVersion)
        {
            var cs = new CameraSettings
            {
                FieldOfView = br.ReadFloat(),
                Height = br.ReadFloat(),
                Pitch = br.ReadFloat(),
                Distance = br.ReadFloat(),
                Stiffness = br.ReadFloat(),
                SwivelSpeed = br.ReadFloat()
            };

            if (engineVersion >= 868 && licenseeVersion >= 20)
            {
                cs.TransitionSpeed = br.ReadFloat();
            }

            return cs;
        }

        public override string ToString()
        {
            return
                $"FieldOfView:{FieldOfView}, Height:{Height}, Pitch:{Pitch}, Distance:{Distance}, Stiffness:{Stiffness}, SwivelSpeed:{SwivelSpeed}, TransitionSpeed:{TransitionSpeed}";
        }
    }
}