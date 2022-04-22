
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class Mask
    {
        World world;

        internal BitSet HasBitSet;
        internal BitSet NotBitSet;
        internal BitSet AnyBitSet;

        internal List<int> Types;

#if DEBUG
        bool isBuilt;
#endif

        public Mask(World world)
        {
            this.world = world;

            Types = new List<int>();

            HasBitSet = new BitSet();
            NotBitSet = new BitSet();
            AnyBitSet = new BitSet();

            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Reset()
        {
            HasBitSet.ClearAll();
            NotBitSet.ClearAll();
            AnyBitSet.ClearAll();

            Types.Clear();

#if DEBUG
            isBuilt = false;
#endif
        }

        public void Has<T>(Entity target) where T : struct
        {
            var index = world.GetStorage<T>(target.Id).Index;

#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(index) != -1)
            {
                throw new Exception($"{typeof(T).Name} already in constraints list.");
            }
#endif
            HasBitSet.Set(index);
            Types.Add(index);
        }

        public void Any<T>(Entity target) where T : struct
        {
            var index = world.GetStorage<T>(target.Id).Index;

#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(index) != -1)
            {
                throw new Exception($"{typeof(T).Name} already in constraints list.");
            }
#endif

            AnyBitSet.Set(index);
            Types.Add(index);
        }

        public void Not<T>(Entity target) where T : struct
        {
            var index = world.GetStorage<T>(target.Id).Index;

#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(index) != -1)
            {
                throw new Exception($"{typeof(T).Name} already in constraints list.");
            }
#endif

            NotBitSet.Set(index);
            Types.Add(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Query Apply(int capacity = 512)
        {
#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            isBuilt = true;
#endif      
            Types.Sort();

            return world.GetQuery(this, capacity);
        }

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