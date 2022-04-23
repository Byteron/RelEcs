using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class BitSet
    {
        const int BitSize = (sizeof(uint) * 8) - 1;
        const int ByteSize = 5;  // log_2(BitSize + 1)

        public int Count { get { return count; } }
        public int Capacity { get { return Bits.Length * (BitSize + 1); } }

        public uint[] Bits = new uint[1];

        int count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Get(int index)
        {
            int b = index >> ByteSize;
            if (b >= Bits.Length)
            {
                return false;
            }

            return (Bits[b] & (1 << (index & BitSize))) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int index)
        {
            int b = index >> ByteSize;
            if (b >= Bits.Length)
            {
                Array.Resize(ref Bits, b + 1);
            }

            Bits[b] |= 1u << (index & BitSize);
            count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(int index)
        {
            int b = index >> ByteSize;
            if (b >= Bits.Length)
            {
                return;
            }

            Bits[b] &= ~(1u << (index & BitSize));
            count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAll()
        {
            Array.Clear(Bits, 0, Bits.Length);
            count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllBitsSet(BitSet mask)
        {
            var count = Math.Min(Bits.Length, mask.Bits.Length);

            for (int i = 0; i < count; i++)
            {
                if ((Bits[i] & mask.Bits[i]) != mask.Bits[i])
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyBitSet(BitSet mask)
        {
            var count = Math.Min(Bits.Length, mask.Bits.Length);

            for (int i = 0; i < count; i++)
            {
                if ((Bits[i] & mask.Bits[i]) != 0)
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
                for (int i = 0; i < Bits.Length; i++)
                {
                    k |= Bits[i];
                }
                return k == 0;
            }
        }

        public override int GetHashCode()
        {
            int h = 1234;
            for (int i = Bits.Length; --i >= 0;)
            {
                h ^= (int)Bits[i] * (i + 1);
            }
            return ((h >> 32) ^ h);
        }
    }
}