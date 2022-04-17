using System;

namespace Bitron.Ecs
{
    internal sealed class BitSet
    {
        const int BitSize = (sizeof(uint) * 8) - 1;
        const int ByteSize = 5;  // log_2(BitSize + 1)

        public int Count { get { return count; } }
        public int Capacity { get { return Bits.Length * (BitSize + 1); } }

        internal uint[] Bits = new uint[1];

        int count = 0;

        public bool Get(int index)
        {
            int b = index >> ByteSize;
            if (b >= Bits.Length)
            {
                return false;
            }

            return (Bits[b] & (1 << (index & BitSize))) != 0;
        }

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

        public void ClearAll()
        {
            Array.Clear(Bits, 0, Bits.Length);
            count = 0;
        }

        // public int[] GetSetIndices()
        // {
        //     List<int> indices = new List<int>();

        //     for (int i = 0; i < Capacity; i++)
        //     {
        //         if (Get(i))
        //         {
        //             indices.Add(i);
        //         }
        //     }

        //     return indices.ToArray();
        // }

        public bool HasAllBitsSet(BitSet mask)
        {
            var count = MathF.Min(Bits.Length, mask.Bits.Length);

            for (int i = 0; i < count; i++)
            {
                if ((Bits[i] & mask.Bits[i]) != mask.Bits[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasAnyBitSet(BitSet mask)
        {
            var count = MathF.Min(Bits.Length, mask.Bits.Length);

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
    }
}