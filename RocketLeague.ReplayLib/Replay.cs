using System.Collections.Generic;
using System.Linq;
using RocketLeague.ReplayLib.NetworkStream;

namespace RocketLeague.ReplayLib
{
    public class Replay
    {
        public int Part1Length { get; init; }
        public uint Part1Crc { get; init; }
        public uint EngineVersion { get; init; }
        public uint LicenseeVersion { get; init; }
        public uint NetVersion { get; set; }

        // Always the string "TAGame.Replay_Soccar_TA"
        public string TaGameReplaySoccarTa { get; set; }
        public FProperty Properties { get; set; }

        public int Part2Length { get; set; }
        public uint Part2Crc { get; set; }
        public int LevelLength { get; set; }
        public List<Level> Levels { get; set; } = new();
        public int KeyFrameLength { get; set; }
        public List<KeyFrame> KeyFrames { get; set; } = new();
        public int NetworkStreamLength { get; set; }
        public List<Frame> Frames { get; set; } = new();
        public int DebugStringLength { get; set; }
        public List<DebugString> DebugStrings { get; set; } = new();
        public int TickMarkLength { get; set; }
        public List<TickMark> TickMarks { get; set; } = new();
        public int PackagesLength { get; set; }
        public List<string> Packages { get; set; } = new();

        public int ObjectLength { get; set; }
        public string[] Objects { get; set; }
        public int NamesLength { get; set; }
        public string[] Names { get; set; }

        public int ClassIndexLength { get; set; }

        public List<ClassIndex> ClassIndexes { get; set; } = new();

        public int ClassNetCacheLength { get; set; }

        public ClassNetCache[] ClassNetCaches { get; set; }
        public uint Unknown { get; set; }

        public int MaxChannels => Properties["MaxChannels"]?.IntValue ?? 1023;

        public void FixClassParent(string childClassName, string parentClassName)
        {
            var parentClass = ClassNetCaches.SingleOrDefault(cnc => Objects[cnc.ObjectIndex] == parentClassName);
            var childClass = ClassNetCaches.SingleOrDefault(cnc => Objects[cnc.ObjectIndex] == childClassName);
            if (parentClass != null && childClass != null &&
                (childClass.Parent == null || childClass.Parent != parentClass))
            {
                var oldParent = childClass.Parent == null ? "NULL" : Objects[childClass.Parent.ObjectIndex];
                System.Diagnostics.Trace.WriteLine(
                    $"Fixing class {childClassName}, setting its parent to {parentClassName} from {oldParent}");

                childClass.Root = false;
                childClass.Parent?.Children.Remove(childClass);
                childClass.Parent = parentClass;
                parentClass.Children.Add(childClass);
            }
        }

        public void MergeDuplicateClasses()
        {
            // Rarely, a class is defined multiple times. 
            // See replay 5F9D44B6400E284FD15A95AC8D5C5B45 which has 2 entries for TAGame.GameEvent_Soccar_TA
            // Merge their properties and drop the extras to keep everything from starting on fire

            var deletedClasses = new List<ClassNetCache>();

            var groupedClasses = ClassNetCaches.GroupBy(cnc => Objects[cnc.ObjectIndex]);
            foreach (var g in groupedClasses.Where(gc => gc.Count() > 1))
            {
                var goodClass = g.First();
                foreach (var badClass in g.Skip(1))
                {
                    foreach (var (key, value) in badClass.Properties)
                    {
                        goodClass.Properties[key] = value;
                    }

                    deletedClasses.Add(badClass);
                }
            }

            ClassNetCaches = ClassNetCaches.Where(cnc => !deletedClasses.Contains(cnc)).ToArray();
        }
    }
}