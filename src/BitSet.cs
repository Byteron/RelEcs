using System;

namespace Bitron.Ecs
{
    internal sealed class BitSet
    {
        const int BitSize = (sizeof(uint) * 8) - 1;
        const int ByteSize = 5;  // log_2(BitSize + 1)

        public int Count { get { return _count; } }

        private int _count = 0;
        private uint[] _bits = new uint[1];
        

        public bool IsSet(int index)
        {
            int b = index >> ByteSize;
            if (b >= _bits.Length)
            {
                return false;
            }

            return (_bits[b] & (1 << (index & BitSize))) != 0;
        }

        public void SetBit(int index)
        {
            int b = index >> ByteSize;
            if (b >= _bits.Length)
            {
                Array.Resize(ref _bits, b + 1);
            }

            _bits[b] |= 1u << (index & BitSize);
            _count++;
        }

        public void ClearBit(int index)
        {
            int b = index >> ByteSize;
            if (b >= _bits.Length)
            {
                return;
            }

            _bits[b] &= ~(1u << (index & BitSize));
            _count--;
        }

        public void ClearAll()
        {
            Array.Clear(_bits, 0, _bits.Length);
            _count = 0;
        }
    }
}