using System;
using System.Runtime.CompilerServices;

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
        bool Has(int entityId);
        void Remove(int entityId);
    }

    public sealed class Storage<T> : IStorage where T : struct
    {
        public int Index { get; set; }
        public long TypeId { get; set; }

        int[] indices = null;
        int[] unusedIds = null;
        int unusedIdCount = 0;

        T[] items = null;
        int count = 0;

        ResetHandler resetDelegate;

        public Storage(WorldConfig config, int index, long typeId)
        {
            indices = new int[config.EntitySize];
            unusedIds = new int[config.StorageSize];
            items = new T[config.StorageSize];

            Index = index;
            TypeId = typeId;

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
        public ref T Add(int entityId)
        {
#if DEBUG
            if (indices[entityId] > 0) { throw new Exception("Already attached."); }
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

            indices[entityId] = index;

            return ref items[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId)
        {
#if DEBUG
            if (indices[entityId] == 0) { throw new Exception("Not attached."); }
#endif
            return ref items[indices[entityId]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetRaw(int entityId)
        {
#if DEBUG
            if (indices[entityId] == 0) { throw new Exception("Not attached."); }
#endif
            return items[indices[entityId]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int entityId)
        {
            ref var index = ref indices[entityId];

            if (index > 0)
            {
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
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entityId)
        {
            return indices[entityId] > 0;
        }

        delegate void ResetHandler(ref T component);
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
    }

    public class TypeIdAssigner
    {
        protected static ushort counter = 0;
    }

    public class TypeIdAssigner<T> : TypeIdAssigner where T : struct
    {
        public static readonly ushort Id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TypeIdAssigner() => Id = ++counter;
    }
}