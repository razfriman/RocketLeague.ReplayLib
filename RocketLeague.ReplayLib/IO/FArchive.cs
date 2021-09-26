using System;
using System.IO;

namespace RocketLeague.ReplayLib.IO
{
    public abstract class FArchive : IDisposable
    {
        public Ue4Version Version { get; set; } = Ue4Version.VerUe4Latest;
        public abstract int Position { get; protected set; }
        public bool IsError { get; protected set; }

        public virtual void SetError()
        {
            IsError = true;
        }

        public void Reset()
        {
            IsError = false;
            Position = 0;
        }

        public abstract bool AtEnd();

        public abstract bool CanRead(int count);

        public abstract T ReadUInt32AsEnum<T>();
        public abstract T ReadByteAsEnum<T>();
        public abstract T[] ReadArray<T>(Func<T> func1);
        public abstract string ReadBytesToString(int count);
        public abstract ushort ReadUInt16();
        public abstract uint ReadUInt32();
        public abstract ulong ReadUInt64();
        public abstract short ReadInt16();
        public abstract int ReadInt32();
        public abstract long ReadInt64();
        public abstract float ReadSingle();
        public abstract string ReadFString();
        public abstract string ReadGuid();
        public abstract string ReadGuid(int size);
        public abstract uint ReadPackedUInt32();
        public abstract ValueTuple<T, TU>[] ReadTupleArray<T, TU>(Func<T> func1, Func<TU> func2);
        public abstract bool ReadBoolean();
        public abstract bool ReadInt32AsBoolean();
        public abstract bool ReadUInt32AsBoolean();
        public abstract byte ReadByte();
        public abstract sbyte ReadSByte();
        public abstract byte[] ReadBytes(int byteCount);
        public abstract byte[] ReadBytes(uint byteCount);
        public abstract void SkipBytes(uint byteCount);
        public abstract void SkipBytes(int byteCount);
        public abstract void Seek(int offset, SeekOrigin seekOrigin = SeekOrigin.Begin);
        public abstract void Dispose();

        public DateTimeOffset ReadDate() => DateTime.FromBinary(ReadInt64()).ToUniversalTime();
    }
}