using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RocketLeague.ReplayLib
{
    public class BitReader
    {
        private readonly BitArray _bits;
        public int Position { get; private set; }
        
        public int Length => _bits.Length;
        
        public BitReader(byte[] bytes)
        {
            _bits = new BitArray(bytes);
        }

        public BitReader(bool[] bits)
        {
            _bits = new BitArray(bits);
        }


        public BitReader(string bitString)
            : this(BitsFromString(bitString))
        {
        }

        public void Seek(int position)
        {
            if (position < 0 || position >= _bits.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            Position = position;
        }

        private static bool[] BitsFromString(string bitString) // Should be a string like "10010010101"
        {
            var bits = new bool[bitString.Length];
            for (var i = 0; i < bitString.Length; ++i)
            {
                bits[i] = bitString[i] switch
                {
                    '0' => false,
                    '1' => true,
                    _ => throw new ArgumentException("Bit string contains characters besides 0 and 1")
                };
            }

            return bits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PeekBit() => _bits[Position];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBit() => _bits[Position++];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte() => ReadBitsAsBytes(8)[0];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32Max(int maxValue)
        {
            var maxBits = Math.Floor(Math.Log10(maxValue) / Math.Log10(2)) + 1;

            uint value = 0;
            for (var i = 0; i < maxBits && value + (1 << i) < maxValue; ++i)
            {
                value += (ReadBit() ? 1U : 0U) << i;
            }

            if (value > maxValue)
            {
                throw new Exception("ReadUInt32Max overflowed!");
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32()
        {
            var value = ReadUInt32FromBits(32);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32() => ReadInt32FromBits(32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64() => ReadUInt32() + ((ulong)ReadUInt32() << 32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadBitsAsBytes(int numBits)
        {
            if (numBits is <= 0 or > 64)
            {
                throw new InvalidOperationException($"Invalid number of bits to read {numBits}");
            }

            var bytes = new byte[(int)Math.Ceiling(numBits / 8.0)];
            var byteIndex = 0;
            var bitIndex = 0;
            for (var i = 0; i < numBits; ++i)
            {
                if (_bits[Position + i])
                {
                    bytes[byteIndex] |= (byte)(1 << bitIndex);
                }

                ++bitIndex;
                if (bitIndex >= 8)
                {
                    ++byteIndex;
                    bitIndex = 0;
                }
            }

            Position += numBits;
            return bytes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32FromBits(int numBits)
        {
            if (numBits is <= 0 or > 32)
                throw new ArgumentException("Number of bits shall be at most 32 bits");
            uint result = 0;
            for (var i = 0; i < numBits; ++i)
            {
                result += (ReadBit() ? 1U : 0U) << i;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32FromBits(int numBits)
        {
            return (int)ReadUInt32FromBits(numBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat()
        {
            var bytes = ReadBitsAsBytes(32);
            return BitConverter.ToSingle(bytes, 0);
        }

        public bool EndOfStream => Position >= _bits.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<bool> GetBits(int startPosition, int count)
        {
            var r = new List<bool>();
            for (var i = 0; i < count; ++i)
            {
                r.Add(_bits[startPosition + i]);
            }

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            for (var i = 0; i < count; ++i)
            {
                bytes[i] = ReadByte();
            }

            return bytes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int? fixedLength = null)
        {
            var length = fixedLength ?? ReadInt32();
            switch (length)
            {
                case > 0:
                {
                    var bytes = ReadBytes(length);
                    return CodePagesEncodingProvider.Instance.GetEncoding(1252).GetString(bytes, 0, length - 1);
                }
                case < 0:
                {
                    var bytes = ReadBytes(length * -2);
                    return Encoding.Unicode.GetString(bytes, 0, length * -2 - 2);
                }
                default:
                    return "";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFixedCompressedFloat(int maxValue, int numBits)
        {
            float value = 0;
            // NumBits = 8:
            var maxBitValue = (1 << (numBits - 1)) - 1; //   0111 1111 - Max abs value we will serialize
            var bias = 1 << (numBits - 1); //   1000 0000 - Bias to pivot around (in order to support signed values)
            var serIntMax = 1 << (numBits - 0); // 1 0000 0000 - What we pass into SerializeInt
            var maxDelta = (1 << (numBits - 0)) - 1; //   1111 1111 - Max delta is

            var delta = (int)ReadUInt32Max(serIntMax);
            float unscaledValue = delta - bias;

            if (maxValue > maxBitValue)
            {
                // We have to scale down, scale needs to be a float:
                var invScale = maxValue / (float)maxBitValue;
                value = unscaledValue * invScale;
            }
            else
            {
                var scale = maxBitValue / maxValue;
                var invScale = 1.0f / scale;

                value = unscaledValue * invScale;
            }

            return value;
        }
    }
}