using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public struct TypeId
    {
        private static Dictionary<long, int> indices = new Dictionary<long, int>();

        private static int count = 0;

        // Id<32Bit> Type<32Bit>
        internal long Value;

        // continuous id;
        internal int Index;

        internal int Entity { get { return (int)(Value >> 32); } }
        internal ushort Type { get { return (ushort)(Value); } }

        internal bool IsPair { get { return Entity != 0; } }

        internal static TypeId Get<T>(Id id = default) where T : struct
        {
            return new TypeId(TypeIdAssigner<T>.Id, id);
        }

        internal TypeId(ushort typeId) : this(typeId, Id.None) { }

        internal TypeId(ushort typeId, Id id = default)
        {
            Value = 0;
            Value |= typeId;
            Value |= ((long)id.Number) << 32;

            if (!indices.TryGetValue(Value, out Index))
            {
                Index = count++;
                indices[Value] = Index;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is TypeId other) && Value == other.Value;
        }

        public static bool operator ==(TypeId left, TypeId right) => left.Equals(right);
        public static bool operator !=(TypeId left, TypeId right) => !left.Equals(right);


        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"(Index: {Index}, Type: {Type}, Entity: {Entity}, Value: {Value})";
        }

        private class TypeIdAssigner
        {
            protected static ushort counter = 1;
        }

        private class TypeIdAssigner<T> : TypeIdAssigner where T : struct
        {
            internal static readonly ushort Id;
            static TypeIdAssigner() => Id = counter++;
        }
    }

    public interface IStorage
    {
        TypeId TypeId { get; set; }
        bool Has(int entityId);
        void Remove(int entityId);
    }

    public sealed class Storage<T> : IStorage where T : struct
    {
        public TypeId TypeId { get; set; }

        int[] indices = null;

        T[] items = null;
        int count = 0;

        internal Storage(World.Config config, TypeId typeId)
        {
            indices = new int[config.EntitySize];
            items = new T[config.StorageSize];
            TypeId = typeId;
        }

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

        public ref T Get(int entityId)
        {
            return ref items[indices[entityId]];
        }

        public object GetRaw(int entityId)
        {
            return items[indices[entityId]];
        }

        public void Remove(int entityId)
        {
            ref var index = ref indices[entityId];

            if (index > 0)
            {
                items[index] = default;
                index = 0;
            }
        }

        public bool Has(int entityId)
        {
            return indices[entityId] > 0;
        }
    }
}