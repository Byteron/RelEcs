using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class BitSet
    {
        const int BitSize = (sizeof(uint) * 8) - 1;
        const int ByteSize = 5;  // log_2(BitSize + 1)

        public int Count { get; private set; }
        
        public int Capacity => bits.Length * (BitSize + 1);
        
        uint[] bits = new uint[1];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Get(int index)
        {
            var b = index >> ByteSize;
            if (b >= bits.Length)
            {
                return false;
            }

            return (bits[b] & (1 << (index & BitSize))) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int index)
        {
            var b = index >> ByteSize;
            if (b >= bits.Length)
            {
                Array.Resize(ref bits, b + 1);
            }

            bits[b] |= 1u << (index & BitSize);
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(int index)
        {
            var b = index >> ByteSize;
            if (b >= bits.Length)
            {
                return;
            }

            bits[b] &= ~(1u << (index & BitSize));
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAll()
        {
            Array.Clear(bits, 0, bits.Length);
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllBitsSet(BitSet mask)
        {
            var min = Math.Min(bits.Length, mask.bits.Length);

            for (var i = 0; i < min; i++)
            {
                if ((bits[i] & mask.bits[i]) != mask.bits[i])
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyBitSet(BitSet mask)
        {
            var min = Math.Min(bits.Length, mask.bits.Length);

            for (var i = 0; i < min; i++)
            {
                if ((bits[i] & mask.bits[i]) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                uint k = 0;
                for (var i = 0; i < bits.Length; i++)
                {
                    k |= bits[i];
                }
                return k == 0;
            }
        }
        
        public override int GetHashCode()
        {
            long h = 1234;
            for (var i = bits.Length; i > 0;)
            {
                h ^= i * bits[--i];
            }
            return (int) ((h >> 32) ^ h);
        }
    }
}