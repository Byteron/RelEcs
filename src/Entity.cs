using System.Collections.Generic;

namespace Bitron.Ecs
{
    public struct Entity
    {
        public int Id;
        public int Gen;
    }

    internal struct EntityMeta
    {
        internal int Id;
        internal int Gen;
        internal BitSet Bitset;
    }

    internal struct RelationMeta
    {
        internal List<int> Entities;

        internal void Add(int entity)
        {
            if (Entities == null)
            {
                Entities = new List<int>();
            }

            Entities.Add(entity);
        }

        internal void Remove(int entity)
        {
            if (Entities == null)
            {
                Entities = new List<int>();
            }

            Entities.Remove(entity);
        }
    }
}