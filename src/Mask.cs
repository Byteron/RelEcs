
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class Mask
    {
        internal BitSet HasBitSet;
        internal BitSet NotBitSet;
        internal BitSet AnyBitSet;

        internal List<int> Types;

#if DEBUG
        bool isBuilt;
#endif

        public Mask()
        {
            Types = new List<int>();

            HasBitSet = new BitSet();
            NotBitSet = new BitSet();
            AnyBitSet = new BitSet();
#if DEBUG
            isBuilt = false;
#endif
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Has(int typeIndex)
        {
#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(typeIndex) != -1)
            {
                throw new Exception($"duplicate type in constrains list");
            }
#endif
            HasBitSet.Set(typeIndex);
            Types.Add(typeIndex);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Any(int typeIndex)
        {
#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(typeIndex) != -1)
            {
                throw new Exception($"duplicate type in constrains list");
            }
#endif

            AnyBitSet.Set(typeIndex);
            Types.Add(typeIndex);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Not(int typeIndex)
        {
#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(typeIndex) != -1)
            {
                throw new Exception($"duplicate type in constrains list");
            }
#endif

            NotBitSet.Set(typeIndex);
            Types.Add(typeIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Lock(int capacity = 512)
        {
#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            isBuilt = true;
#endif
            Types.Sort();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            int hash = Types.Count;

            foreach (var index in Types)
            {
                hash = unchecked(hash * HasBitSet.GetHashCode() + index);
                hash = unchecked(hash * NotBitSet.GetHashCode() + index);
                hash = unchecked(hash * AnyBitSet.GetHashCode() + index);
            }

            return hash;
        }
    }
}