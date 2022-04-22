using System;
using System.Runtime.CompilerServices;

namespace Bitron.Ecs
{
    public struct IsA { }
    public struct ChildOf { }

    public interface IStorage
    {
        int Index { get; set; }
        long TypeId { get; set; }
        bool Has(int entityId);
        void Remove(int entityId);
    }

    public sealed class Storage<T> : IStorage where T : struct
    {
        public int Index { get; set; }
        public long TypeId { get; set; }

        int[] indices = null;

        T[] items = null;
        int count = 0;

        public Storage(WorldConfig config, int index, long typeId)
        {
            indices = new int[config.EntitySize];
            items = new T[config.StorageSize];

            Index = index;
            TypeId = typeId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add(int entityId)
        {
            int index = count++;

            if (entityId >= indices.Length)
            {
                Array.Resize(ref indices, entityId << 1);
            }

            if (count == items.Length)
            {
                Array.Resize(ref items, count << 1);
            }

            indices[entityId] = index;
            return ref items[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId)
        {
            return ref items[indices[entityId]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetRaw(int entityId)
        {
            return items[indices[entityId]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int entityId)
        {
            ref var index = ref indices[entityId];

            if (index > 0)
            {
                items[index] = default;
                index = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entityId)
        {
            return indices[entityId] > 0;
        }
    }

    public static class TypeId
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Value<T>(int entityId) where T : struct
        {
            return (long)TypeIdAssigner<T>.Id | (long)entityId << 32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Entity(long value)
        {
            return (int)(value >> 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Type(long value)
        {
            return (ushort)value;
        }

        class TypeIdAssigner
        {
            protected static ushort counter = 0;
        }

        class TypeIdAssigner<T> : TypeIdAssigner where T : struct
        {
            public static readonly ushort Id;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static TypeIdAssigner() => Id = ++counter;
        }
    }
}