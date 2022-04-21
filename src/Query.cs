
using System;
using System.Runtime.CompilerServices;

namespace Bitron.Ecs
{
    public sealed class Query
    {
        public World World;

        private int[] indices;
        private Entity[] entities;
        public int entityCount;

        Mask mask;

        public Query(World world, Mask mask, int entitySize)
        {
            World = world;
            indices = new int[entitySize];
            entities = new Entity[entitySize];
            this.mask = mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEntity(Entity entity)
        {
            var index = entityCount++;
            indices[entityCount] = index;

            if (entityCount == entities.Length)
            {
                Array.Resize(ref entities, entityCount << 1);
            }

            entities[index] = entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveEntity(EntityId entityId)
        {
            var index = indices[entityId.Number];
            indices[entityId.Number] = 0;

            entityCount--;

            if (index < entityCount)
            {
                entities[index] = entities[entityCount];
                indices[entities[index].Id.Number] = index;
            }
        }

        public override int GetHashCode()
        {
            return mask.GetHashCode();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(entities, entityCount);
        }

        public struct Enumerator : IDisposable
        {
            private readonly Entity[] entities;
            private readonly int count;
            private int index;

            public Enumerator(Entity[] entities, int count)
            {
                this.entities = entities;
                this.count = count;
                index = -1;
            }

            public int Count {  get { return count; } }

            public Entity Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => entities[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                return ++index < count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose() { }
        }
    }

    public sealed class Mask
    {
        private World world;

        public BitSet IncludeBitSet = new BitSet();
        public BitSet ExcludeBitSet = new BitSet();

        public Mask(World world)
        {
            this.world = world;
        }

        public void With<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);
            IncludeBitSet.Set(index);
        }

        public void Without<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);
            ExcludeBitSet.Set(index);
        }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                int hashcode = 1430287;
                hashcode = hashcode * 7302013 ^ IncludeBitSet.GetHashCode();
                hashcode = hashcode * 7302013 ^ ExcludeBitSet.GetHashCode();
                return hashcode;
            }
        }
    }
}