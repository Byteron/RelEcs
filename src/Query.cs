using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class Query
    {
        World world;

        int[] indices = new int[512];
        Entity[] entities = new Entity[512];

        int entityCount = 0;

        internal Query(World world)
        {
            this.world = world;
        }

        public void AddEntity(Entity entity)
        {

            if (entityCount == entities.Length)
            {
                Array.Resize(ref entities, entityCount << 1);
            }
            int index = ++entityCount;

            entities[index] = entity;
            indices[entity.Id] = index;
        }

        public void RemoveEntity(Entity entity)
        {
            var index = indices[entity.Id];
            indices[entity.Id] = 0;

            entityCount--;

            if (index < entityCount)
            {
                entities[index] = entities[entityCount];
                indices[entities[index].Id] = index;
            }
        }
    }

    internal sealed class Mask
    {
        internal Dictionary<int, int> RelationMap { get; private set; } = new Dictionary<int, int>();
        
        BitSet includeBitSet = new BitSet();
        BitSet excludeBitSet = new BitSet();


        // BitSet addedBitSet = new BitSet();
        // BitSet removedBitSet = new BitSet();

        internal void With<T>() where T : struct, IComponent
        {
            var typeId = ComponentType<T>.Id;
            includeBitSet.Set(typeId);
        }

        internal void IsA<T>() where T : struct, IRelation
        {
            var typeId = ComponentType<Relation<T>>.Id;
            includeBitSet.Set(typeId);
        }

        internal void IsA<T>(Entity target) where T : struct, IRelation
        {
            var typeId = ComponentType<Relation<T>>.Id;
            RelationMap.Add(target.Id, typeId);
        }

        internal void Without<T>() where T : struct
        {
            var typeId = ComponentType<T>.Id;
            excludeBitSet.Set(typeId);
        }

        // internal void Added<T>() where T : struct
        // {
        //     var typeId = ComponentType<T>.Id;
        //     addedBitSet.Set(typeId);
        // }

        // internal void Removed<T>() where T : struct
        // {
        //     var typeId = ComponentType<T>.Id;
        //     removedBitSet.Set(typeId);
        // }

        internal bool IsCompatibleWith(BitSet bitSet)
        {
            if (!bitSet.HasAllBitsSet(includeBitSet))
            {
                return false;
            }

            if (bitSet.HasAnyBitSet(excludeBitSet))
            {
                return false;
            }

            return true;
        }
    }
}