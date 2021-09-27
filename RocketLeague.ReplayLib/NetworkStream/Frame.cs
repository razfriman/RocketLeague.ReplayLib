using System;
using System.Collections.Generic;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class Frame
    {
        public int Position { get; private set; }
        public float Time { get; private set; }
        public float Delta { get; private set; }
        public int BitLength { get; private set; }


        public List<ActorState> ActorStates { get; private set; }

        public static Frame Deserialize(int maxChannels, ref Dictionary<uint, ActorState> existingActorStates,
            string[] objectIdToName, IDictionary<string, ClassNetCache> classNetCacheByName, uint engineVersion,
            uint licenseeVersion, uint netVersion, BitReader br)
        {
            var f = new Frame
            {
                Position = br.Position,
                Time = br.ReadFloat(),
                Delta = br.ReadFloat()
            };


            if (f.Time < 0 || f.Delta < 0
                           || f.Time > 0 && f.Time < 1E-10
                           || f.Delta > 0 && f.Delta < 1E-10)
            {
                var error =
                    $"\"Frame\" at postion {f.Position} has time values that are negative or suspicious. The parser got lost. Check the previous frame for bad data. Time {f.Time}, Delta {f.Delta}";
                throw new Exception(error);
            }


            f.ActorStates = new List<ActorState>();

            ActorState lastActorState = null;
            while (br.ReadBit())
            {
                lastActorState = ActorState.Deserialize(maxChannels, existingActorStates, f.ActorStates, objectIdToName,
                    classNetCacheByName, engineVersion, licenseeVersion, netVersion, br);

                existingActorStates.TryGetValue(lastActorState.Id, out var existingActor);
                if (lastActorState.State != ActorStateState.Deleted)
                {
                    if (existingActor == null)
                    {
                        existingActorStates[lastActorState.Id] = lastActorState;
                    }
                }
                else
                {
                    existingActorStates.Remove(lastActorState.Id);
                }

                f.ActorStates.Add(lastActorState);
            }

            return f;
        }
    }
}