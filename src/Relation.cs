using System;

namespace Bitron.Ecs
{
    public interface IRelation { }

    public interface IRelationStorage
    {
        int TypeId { get; set; }
        Entity GetEntity(int entityId);
        void Remove(int entityId);
    }

    public sealed class RelationStorage<T> : IRelationStorage where T : struct
    {
        public int TypeId { get; set; }

        int[] indices = new int[512];
        Relation<T>[] items = new Relation<T>[512];
        int count = 0;


        internal RelationStorage(int typeId)
        {
            TypeId = typeId;
        }

        public ref Relation<T> Add(int entityId)
        {
            int index = count++;

            if (count == items.Length)
            {
                Array.Resize(ref items, count << 1);
            }

            indices[entityId] = index;
            return ref items[index];
        }

        public ref Relation<T> Get(int entityId)
        {
            return ref items[indices[entityId]];
        }

        public Entity GetEntity(int entityId)
        {
            return items[indices[entityId]].Entity;
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

    public class Relation<T>
    {
        public T Data;
        public Entity Entity;
    }
}