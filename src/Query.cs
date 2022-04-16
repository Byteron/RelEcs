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
        internal List<(Entity, int)> Relations { get; private set; } = new List<(Entity, int)>();
        
        internal BitSet IncludeBitSet = new BitSet();
        internal BitSet ExcludeBitSet = new BitSet();


        // BitSet addedBitSet = new BitSet();
        // BitSet removedBitSet = new BitSet();

        internal void With<T>() where T : struct, IComponent
        {
            var typeId = TypeIdAssigner<T>.Id;
            IncludeBitSet.Set(typeId);
        }

        internal void Without<T>() where T : struct
        {
            var typeId = TypeIdAssigner<T>.Id;
            ExcludeBitSet.Set(typeId);
        }

        internal void IsA<T>() where T : struct, IRelation
        {
            var typeId = TypeIdAssigner<T>.Id;
            IncludeBitSet.Set(typeId);
        }

        internal void IsA<T>(Entity target) where T : struct, IRelation
        {
            var typeId = TypeIdAssigner<T>.Id;
            Relations.Add((target, typeId));
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
    }
}