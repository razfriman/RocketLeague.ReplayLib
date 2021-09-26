using System;

namespace RocketLeague.ReplayLib.Memory
{
    public interface IPinnedMemoryOwner<T> : IDisposable
    {
        public PinnedMemory<T> PinnedMemory { get; }
    }
}