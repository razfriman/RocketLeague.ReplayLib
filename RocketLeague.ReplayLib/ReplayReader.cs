using System;
using System.Diagnostics;
using System.IO;
using RocketLeague.ReplayLib.IO;
using RocketLeague.ReplayLib.Models;
using Microsoft.Extensions.Logging;

namespace RocketLeague.ReplayLib
{
    public abstract class ReplayReader<T> where T : Replay, new()
    {
        protected readonly ILogger Logger;
        protected T Replay { get; set; }
        protected bool IsReading { get; set; }
        protected readonly NetBitReader FrameReader = new();
        
        protected ReplayReader(ILogger logger)
        {
            Logger = logger;
        }

        private void Reset() => Replay = new T();

        public T ReadReplay(string fileName)
        {
            using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return ReadReplay(stream);
        }

        public T ReadReplay(Stream stream)
        {
            using var archive = new UnrealBinaryReader(stream);
            return ReadReplay(archive);
        }

        public T ReadReplay(UnrealBinaryReader archive)
        {
            if (IsReading)
            {
                throw new InvalidOperationException("Multithreaded reading currently isn't supported");
            }

            var sw = Stopwatch.StartNew();
            try
            {
                Reset();
                OnStartParse();
                IsReading = true;
                ReadReplayData(archive);
                Cleanup();
                Replay.ParseTime = sw.ElapsedMilliseconds;
                OnFinishParse();
                return Replay;
            }
            finally
            {
                IsReading = false;
            }
        }

        private void ReadReplayData(UnrealBinaryReader reader)
        {
            var part1Length = reader.ReadInt32();
            var part1Crc = reader.ReadUInt32();
            var position1 = reader.Position;
            ReadReplayHeaderData(reader);
            reader.Seek(position1 + part1Length);

            var part2Length = reader.ReadInt32();
            var part2Crc = reader.ReadUInt32();
            var position2 = reader.Position;
            var (networkStreamPosition, networkStreamLength) = ReadReplayObjectData(reader);
            reader.Seek(position2 + part2Length);

            // Replay.PropertyContainer
            //ReadReplayFrames(reader, networkStreamPosition, networkStreamLength);
        }

        public class ActorState
        {
        }

        private unsafe void ReadReplayFrames(UnrealBinaryReader reader, int networkStreamPosition, int networkStreamLength)
        {
            var maxChannels = Replay.PropertyContainer["MaxChannels"]?.IntValue ?? 1023;
            var byteLength = networkStreamLength;
            var bitLength = byteLength * 8;
            reader.Seek(networkStreamPosition);
            using var buffer = reader.GetMemoryBuffer(networkStreamLength);
            FrameReader.SetBits(buffer.PositionPointer, networkStreamLength, networkStreamLength * 8);
            
            
            while (FrameReader.Position < (bitLength - 64))
            {
                var frame = new Frame();
                var position = FrameReader.Position;
                var time = FrameReader.ReadSingle();
                var delta = FrameReader.ReadSingle();
                Console.WriteLine($"{time} : {delta}");
                while (FrameReader.ReadBit())
                {
                    var actorId = FrameReader.ReadInt32Max(maxChannels);

                    var isAlive = FrameReader.ReadBit();
                    if (isAlive)
                    {
                        var isNewActor = FrameReader.ReadBit();
                        if (isNewActor)
                        {
                            var nameId = FrameReader.ReadInt32();
                            FrameReader.SkipBits(1);
                            var objectId = FrameReader.ReadInt32();
                            var actorPosition = FrameReader.ReadPackedVector(1, 22);
                            //if (ClassHasRotation(objectIndexToName[a.ClassId.Value]))
                            if (false)
                            {
                                var actorRotation = FrameReader.ReadRotation();
                            }
                        }
                        else
                        {
                            var maxPropertyId = 10; // classMap.MaxPropertyId;
                            while (FrameReader.ReadBit())
                            {
                                var propertyId = FrameReader.ReadInt32Max(maxPropertyId + 1);
                                var propertyName = ""; // objectIndexToName[classMap.GetProperty((int)asp.PropertyId).Index];

                                switch (propertyName)
                                {
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        
                    }
                    // FrameReader.ReadBits()
                    //         let actor_id = bits
                    //     .read_bits_max_computed(self.channel_bits, u64::from(self.max_channels))
                    //     .map(|x| ActorId(x as i32))
                    //     .ok_or(FrameError::NotEnoughDataFor("Actor Id"))?;
                    //
                    // // alive
                    // if bits
                    //     .read_bit()
                    //     .ok_or(FrameError::NotEnoughDataFor("Is actor alive"))?
                    // {
                    //     // new
                    //     if bits
                    //         .read_bit()
                    //         .ok_or(FrameError::NotEnoughDataFor("Is new actor"))?
                    //     {
                    //         let actor = self.parse_new_actor(&mut bits, actor_id)?;
                    //
                    //         // Insert the new actor so we can keep track of it for attribute
                    //         // updates. It's common for an actor id to already exist, so we
                    //         // overwrite it.
                    //         actors.insert(actor.actor_id, actor.object_id);
                    //         new_actors.push(actor);
                    //     } else {
                    //         // We'll be updating an existing actor with some attributes so we need
                    //         // to track down what the actor's type is
                    //         let object_id = actors
                    //             .get(&actor_id)
                    //             .ok_or(FrameError::MissingActor { actor: actor_id })?;
                    //
                    //         // Once we have the type we need to look up what attributes are
                    //         // available for said type
                    //         let cache_info =
                    //             self.object_ind_attributes.get(object_id).ok_or_else(|| {
                    //                 FrameError::MissingCache {
                    //                     actor: actor_id,
                    //                     actor_object: *object_id,
                    //                 }
                    //             })?;
                    //
                    //         // While there are more attributes to update for our actor:
                    //         while bits
                    //             .read_bit()
                    //             .ok_or(FrameError::NotEnoughDataFor("Is prop present"))?
                    //         {
                    //             // We've previously calculated the max the stream id can be for a
                    //             // given type and how many bits that it encompasses so use those
                    //             // values now
                    //             let stream_id = bits
                    //                 .read_bits_max_computed(
                    //                     cache_info.prop_id_bits,
                    //                     u64::from(cache_info.max_prop_id),
                    //                 )
                    //                 .map(|x| StreamId(x as i32))
                    //                 .ok_or(FrameError::NotEnoughDataFor("Prop id"))?;
                    //
                    //             // Look the stream id up and find the corresponding attribute
                    //             // decoding function. Experience has told me replays that fail to
                    //             // parse, fail to do so here, so a large chunk is dedicated to
                    //             // generating an error message with context
                    //             let attr = cache_info.attributes.get(&stream_id).ok_or_else(|| {
                    //                 FrameError::MissingAttribute {
                    //                     actor: actor_id,
                    //                     actor_object: *object_id,
                    //                     attribute_stream: stream_id,
                    //                 }
                    //             })?;
                    //
                    //             let attribute = attr_decoder
                    //                 .decode(attr.attribute, &mut bits, buf)
                    //                 .map_err(|e| match e {
                    //                     AttributeError::Unimplemented => FrameError::MissingAttribute {
                    //                         actor: actor_id,
                    //                         actor_object: *object_id,
                    //                         attribute_stream: stream_id,
                    //                     },
                    //                     e => FrameError::AttributeError {
                    //                         actor: actor_id,
                    //                         actor_object: *object_id,
                    //                         attribute_stream: stream_id,
                    //                         error: e,
                    //                     },
                    //                 })?;
                    //
                    //             updated_actors.push(UpdatedAttribute {
                    //                 actor_id,
                    //                 stream_id,
                    //                 object_id: attr.object_id,
                    //                 attribute,
                    //             });
                    // }
                    // }
                }
            }
            Console.WriteLine("done");
        }

        private void ReadReplayHeaderData(UnrealBinaryReader reader)
        {
            Replay.EngineVersion = reader.ReadUInt32();
            Replay.LicenseeVersion = reader.ReadUInt32();
            if (Replay.EngineVersion >= 868 && Replay.LicenseeVersion >= 18)
            {
                Replay.NetVersion = reader.ReadUInt32();
            }

            Replay.ReplayClass = reader.ReadFString();
            Replay.PropertyContainer = FProperty.DeserializePropertyContainer(reader);
            var json = Replay.PropertyContainer.ToJson();
            Console.WriteLine(json);
            OnReadReplayHeaderData();
        }

        private (int networkStreamPosition, int networkStreamLength) ReadReplayObjectData(UnrealBinaryReader reader)
        {
            var levelCount = reader.ReadInt32();

            for (var i = 0; i < levelCount; i++)
            {
                Replay.Levels.Add(new Level
                {
                    Name = reader.ReadFString()
                });
            }

            var keyFrameCount = reader.ReadInt32();
            for (var i = 0; i < keyFrameCount; i++)
            {
                Replay.KeyFrames.Add(new KeyFrame
                {
                    Time = reader.ReadSingle(),
                    Frame = reader.ReadInt32(),
                    FilePosition = reader.ReadInt32()
                });
            }

            var networkStreamLength = reader.ReadInt32();
            var networkStreamPosition = reader.Position;
            reader.SkipBytes(networkStreamLength);

            var debugStringCount = reader.ReadInt32();
            for (var i = 0; i < debugStringCount; i++)
            {
                Replay.DebugStrings.Add(new DebugString
                {
                    FrameNumber = reader.ReadInt32(),
                    Username = reader.ReadFString(),
                    Text = reader.ReadFString()
                });
            }

            var tickMarkCount = reader.ReadInt32();
            for (var i = 0; i < tickMarkCount; i++)
            {
                Replay.TickMarks.Add(new TickMark
                {
                    Type = reader.ReadFString(),
                    Frame = reader.ReadInt32()
                });
            }

            var packageCount = reader.ReadInt32();
            for (var i = 0; i < packageCount; i++)
            {
                Replay.Packages.Add(reader.ReadFString());
            }

            var objectCount = reader.ReadInt32();
            for (var i = 0; i < objectCount; i++)
            {
                Replay.Objects.Add(reader.ReadFString());
            }

            var nameCount = reader.ReadInt32();
            for (var i = 0; i < nameCount; i++)
            {
                Replay.Names.Add(reader.ReadFString());
            }

            var classIndexCount = reader.ReadInt32();
            for (var i = 0; i < classIndexCount; i++)
            {
                Replay.ClassIndexes.Add(new ClassIndex
                {
                    Name = reader.ReadFString(),
                    Index = reader.ReadInt32()
                });
            }

            var classNetCacheCount = reader.ReadInt32();
            for (var i = 0; i < classNetCacheCount; i++)
            {
                var classNetCache = new ClassNetCache
                {
                    ObjectIndex = reader.ReadInt32(),
                    ParentId = reader.ReadInt32(),
                    ClassNetCacheId = reader.ReadInt32()
                };

                var propertiesLength = reader.ReadInt32();
                for (var j = 0; j < propertiesLength; ++j)
                {
                    classNetCache.Properties.Add(new ClassNetCacheProperty
                    {
                        PropertyIndex = reader.ReadInt32(),
                        PropertyId = reader.ReadInt32()
                    });
                }

                Replay.ClassNetCaches.Add(classNetCache);
            }

            OnReadReplayObjectData();

            return (networkStreamPosition, networkStreamLength);
        }

        protected void Cleanup()
        {
        }

        protected virtual void OnStartParse()
        {
        }

        protected virtual void OnFinishParse()
        {
        }

        protected virtual void OnReadReplayHeaderData()
        {
        }

        protected virtual void OnReadReplayObjectData()
        {
        }
    }
}