using System;
using System.Runtime.CompilerServices;
using Godot;

namespace RelEcs
{
    public struct IsA { }
    public struct ChildOf { }

    public interface IReset<T> where T : struct
    {
        void Reset(ref T c);
    }

    public interface IStorage
    {
        int Index { get; set; }
        long TypeId { get; set; }
        Type Type { get; set; }

        bool Has(int id);
        void Remove(int id);
        void Resize(int capacity);
    }

    public sealed class Storage<T> : IStorage where T : struct
    {
        public int Index { get; set; }
        public long TypeId { get; set; }
        public Type Type { get; set; }

        int[] indices;
        int[] unusedIds;
        int unusedIdCount;

        T[] items;
        int count;

        ResetHandler resetDelegate;

        public Storage(WorldConfig config, int index, long typeId, int capacity)
        {
            indices = new int[capacity];
            unusedIds = new int[config.StorageSize];
            items = new T[config.StorageSize];

            Index = index;
            TypeId = typeId;
            Type = typeof(T);

            var isAutoReset = typeof(IReset<T>).IsAssignableFrom(typeof(T));
#if DEBUG
            if (!isAutoReset && typeof(T).GetInterface("IReset`1") != null)
            {
                throw new Exception($"IReset should have <{typeof(T).Name}> constraint for component \"{typeof(T).Name}\".");
            }
#endif
            if (isAutoReset)
            {
                var autoResetMethod = typeof(T).GetMethod(nameof(IReset<T>.Reset));
#if DEBUG
                if (autoResetMethod == null)
                {
                    throw new Exception(
                        $"IReset<{typeof(T).Name}> explicit implementation not supported, use implicit instead.");
                }
#endif
                resetDelegate = (ResetHandler)Delegate.CreateDelegate(typeof(ResetHandler), null, autoResetMethod);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add(int id)
        {
            if (id >= indices.Length)
            {
                Array.Resize(ref indices, id << 1);
            }

#if DEBUG
            if (indices[id] > 0) { throw new Exception($"{typeof(T).Name} ({Index}) is already attached to {id}"); }
#endif
            int index;

            if (unusedIdCount > 0)
            {
                index = unusedIds[--unusedIdCount];
            }
            else
            {
                index = ++count;
                if (count == items.Length)
                {
                    Array.Resize(ref items, count << 1);
                }
                resetDelegate?.Invoke(ref items[index]);
            }

            indices[id] = index;

            return ref items[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get(int id)
        {
#if DEBUG
            if (indices[id] == 0) { throw new Exception($"{typeof(T).Name} ({Index}) is not attached to {id}"); }
#endif
            return ref items[indices[id]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int id)
        {
            ref var index = ref indices[id];

            if (index <= 0) return;
            
            if (unusedIdCount == unusedIds.Length)
            {
                Array.Resize(ref unusedIds, unusedIdCount << 1);
            }

            unusedIds[unusedIdCount++] = index;

            if (resetDelegate != null)
            {
                resetDelegate.Invoke(ref items[index]);
            }
            else
            {
                items[index] = default;
            }

            index = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(int capacity)
        {
            Array.Resize(ref indices, capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int id)
        {
            return indices[id] > 0;
        }

        delegate void ResetHandler(ref T component);
    }

    public static class TypeId
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Value<T>(int id) where T : struct
        {
            return TypeIdAssigner<T>.Id | (long)id << 32;
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
        
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool IsTag<T>() where T : struct
        // {
        //     return TypeIdAssigner<T>.IsTag;
        // }

        class TypeIdAssigner
        {
            protected static ushort Counter = 0;
        }

        class TypeIdAssigner<T> : TypeIdAssigner where T : struct
        {
            public static readonly ushort Id;
            // public static readonly bool IsTag;
        

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static TypeIdAssigner() 
            {
                Id = ++Counter;
                // IsTag = Unsafe.SizeOf<T>() == 1;
            }
        }
    }
}