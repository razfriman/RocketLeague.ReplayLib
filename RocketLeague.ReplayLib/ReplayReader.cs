using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RocketLeague.ReplayLib.Extensions;
using RocketLeague.ReplayLib.NetworkStream;

namespace RocketLeague.ReplayLib
{
    public class ReplayReader
    {
        public Replay Deserialize(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Deserialize(fs);
        }

        public Replay Deserialize(Stream stream)
        {
            using var br = new BinaryReader(stream);
            return Deserialize(br);
        }

        public Replay DeserializeHeader(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var br = new BinaryReader(fs);
            return DeserializeHeader(br);
        }

        public Replay DeserializeHeader(BinaryReader br)
        {
            var replay = new Replay
            {
                Part1Length = br.ReadInt32(),
                Part1Crc = br.ReadUInt32(),
                EngineVersion = br.ReadUInt32(),
                LicenseeVersion = br.ReadUInt32()
            };

            if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18)
            {
                replay.NetVersion = br.ReadUInt32();
            }

            replay.TaGameReplaySoccarTa = br.ReadFString();

            replay.Properties = FPropertyParser.DeserializePropertyContainer(br);

            return replay;
        }

        public Replay Deserialize(BinaryReader br)
        {
            var replay = DeserializeHeader(br);

            replay.Part2Length = br.ReadInt32();
            replay.Part2Crc = br.ReadUInt32();

            replay.LevelLength = br.ReadInt32();
            // looks like sfx data, not level data. shrug
            replay.Levels = new List<Level>();
            for (var i = 0; i < replay.LevelLength; i++)
            {
                replay.Levels.Add(Level.Deserialize(br));
            }

            replay.KeyFrameLength = br.ReadInt32();
            replay.KeyFrames = new List<KeyFrame>();
            for (var i = 0; i < replay.KeyFrameLength; i++)
            {
                replay.KeyFrames.Add(KeyFrame.Deserialize(br));
            }

            replay.NetworkStreamLength = br.ReadInt32();

            var networkStream = ArrayPool<byte>.Shared.Rent(replay.NetworkStreamLength);
            br.Read(networkStream.AsSpan(0, replay.NetworkStreamLength));

            replay.DebugStringLength = br.ReadInt32();
            for (var i = 0; i < replay.DebugStringLength; i++)
            {
                replay.DebugStrings.Add(DebugString.Deserialize(br));
            }

            replay.TickMarkLength = br.ReadInt32();
            replay.TickMarks = new List<TickMark>();
            for (var i = 0; i < replay.TickMarkLength; i++)
            {
                replay.TickMarks.Add(TickMark.Deserialize(br));
            }

            replay.PackagesLength = br.ReadInt32();
            replay.Packages = new List<string>();
            for (var i = 0; i < replay.PackagesLength; i++)
            {
                replay.Packages.Add(br.ReadFString());
            }

            replay.ObjectLength = br.ReadInt32();
            replay.Objects = new string[replay.ObjectLength];
            for (var i = 0; i < replay.ObjectLength; i++)
            {
                replay.Objects[i] = br.ReadFString();
            }

            replay.NamesLength = br.ReadInt32();
            replay.Names = new string[replay.NamesLength];
            for (var i = 0; i < replay.NamesLength; i++)
            {
                replay.Names[i] = br.ReadFString();
            }

            replay.ClassIndexLength = br.ReadInt32();
            replay.ClassIndexes = new List<ClassIndex>();
            for (var i = 0; i < replay.ClassIndexLength; i++)
            {
                replay.ClassIndexes.Add(ClassIndex.Deserialize(br));
            }

            replay.ClassNetCacheLength = br.ReadInt32();
            replay.ClassNetCaches = new ClassNetCache[replay.ClassNetCacheLength];
            for (var i = 0; i < replay.ClassNetCacheLength; i++)
            {
                var classNetCache = ClassNetCache.Deserialize(br);
                replay.ClassNetCaches[i] = classNetCache;

                for (var j = i - 1; j >= 0; --j)
                {
                    if (classNetCache.ParentId == replay.ClassNetCaches[j].Id)
                    {
                        classNetCache.Parent = replay.ClassNetCaches[j];
                        replay.ClassNetCaches[j].Children.Add(classNetCache);
                        break;
                    }
                }

                if (replay.ClassNetCaches[i].Parent == null)
                {
                    replay.ClassNetCaches[i].Root = true;
                }
            }

            if (replay.NetVersion >= 10)
            {
                replay.Unknown = br.ReadUInt32();
            }

            replay.MergeDuplicateClasses();
            replay.FixClassParent("ProjectX.PRI_X", "Engine.PlayerReplicationInfo");
            replay.FixClassParent("TAGame.PRI_TA", "ProjectX.PRI_X");
            replay.FixClassParent("TAGame.CarComponent_Boost_TA", "TAGame.CarComponent_TA");
            replay.FixClassParent("TAGame.CarComponent_FlipCar_TA", "TAGame.CarComponent_TA");
            replay.FixClassParent("TAGame.CarComponent_Jump_TA", "TAGame.CarComponent_TA");
            replay.FixClassParent("TAGame.CarComponent_Dodge_TA", "TAGame.CarComponent_TA");
            replay.FixClassParent("TAGame.CarComponent_DoubleJump_TA", "TAGame.CarComponent_TA");
            replay.FixClassParent("TAGame.GameEvent_TA", "Engine.Actor");
            replay.FixClassParent("TAGame.SpecialPickup_TA", "TAGame.CarComponent_TA");
            replay.FixClassParent("TAGame.SpecialPickup_BallVelcro_TA", "TAGame.SpecialPickup_TA");
            replay.FixClassParent("TAGame.SpecialPickup_Targeted_TA", "TAGame.SpecialPickup_TA");
            replay.FixClassParent("TAGame.SpecialPickup_Spring_TA", "TAGame.SpecialPickup_Targeted_TA");
            replay.FixClassParent("TAGame.SpecialPickup_BallLasso_TA", "TAGame.SpecialPickup_Spring_TA");
            replay.FixClassParent("TAGame.SpecialPickup_BoostOverride_TA", "TAGame.SpecialPickup_Targeted_TA");
            replay.FixClassParent("TAGame.SpecialPickup_BallCarSpring_TA", "TAGame.SpecialPickup_Spring_TA");
            replay.FixClassParent("TAGame.SpecialPickup_BallFreeze_TA", "TAGame.SpecialPickup_Targeted_TA");
            replay.FixClassParent("TAGame.SpecialPickup_Swapper_TA", "TAGame.SpecialPickup_Targeted_TA");
            replay.FixClassParent("TAGame.SpecialPickup_GrapplingHook_TA", "TAGame.SpecialPickup_Targeted_TA");
            replay.FixClassParent("TAGame.SpecialPickup_BallGravity_TA", "TAGame.SpecialPickup_TA");
            replay.FixClassParent("TAGame.SpecialPickup_HitForce_TA", "TAGame.SpecialPickup_TA");
            replay.FixClassParent("TAGame.SpecialPickup_Tornado_TA", "TAGame.SpecialPickup_TA");
            replay.FixClassParent("TAGame.SpecialPickup_HauntedBallBeam_TA", "TAGame.SpecialPickup_TA");
            replay.FixClassParent("TAGame.CarComponent_TA", "Engine.Actor");
            replay.FixClassParent("Engine.Info", "Engine.Actor");
            replay.FixClassParent("Engine.Pawn", "Engine.Actor");
            replay.FixClassParent("Engine.TeamInfo", "Engine.ReplicationInfo");
            replay.FixClassParent("TAGame.Team_TA", "Engine.TeamInfo");

            replay.Frames = ExtractFrames(replay.MaxChannels, networkStream, replay.Objects,
                replay.ClassNetCaches, replay.EngineVersion, replay.LicenseeVersion, replay.NetVersion);
            
            ArrayPool<byte>.Shared.Return(networkStream);

            if (br.BaseStream.Position != br.BaseStream.Length)
            {
                throw new Exception("Extra data somewhere!");
            }

            return replay;
        }

        private static List<Frame> ExtractFrames(int maxChannels, byte[] networkStream,
            string[] objectIdToName, IEnumerable<ClassNetCache> classNetCache, uint engineVersion, uint licenseeVersion,
            uint netVersion)
        {
            Dictionary<uint, ActorState> actorStates = new();

            IDictionary<string, ClassNetCache> classNetCacheByName =
                classNetCache.ToDictionary(k => objectIdToName[k.ObjectIndex], v => v);

            var br = new BitReader(networkStream);
            List<Frame> frames = new();

            while (br.Position < br.Length - 64)
            {
                var newFrame = Frame.Deserialize(maxChannels, ref actorStates, objectIdToName, classNetCacheByName,
                    engineVersion, licenseeVersion, netVersion, br);

                if (frames.Any() && newFrame.Time != 0 && newFrame.Time < frames.Last().Time
                )
                {
                    var error =
                        $"Frame time is less than the previous frame's time. Parser is lost. Frame position {newFrame.Position}, Time {newFrame.Time}. Previous frame time {frames.Last().Time}";
                    throw new Exception(error);
                }

                frames.Add(newFrame);
            }

            return frames;
        }
    }
}