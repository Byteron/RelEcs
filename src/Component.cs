using System;

namespace Bitron.Ecs
{
    public interface IComponent { }

    public interface IComponentStorage
    {
        int TypeId { get; set; }
        void Remove(int entityId);
    }

    public sealed class ComponentStorage<T> : IComponentStorage where T : struct
    {
        public int TypeId { get; set; }

        int[] indices = new int[512];
        T[] items = new T[512];
        int count = 0;


        internal ComponentStorage(int typeId)
        {
            TypeId = typeId;
        }

        public ref T Add(int entityId)
        {
            int index = count++;

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