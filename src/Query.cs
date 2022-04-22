
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bitron.Ecs
{
    public sealed class Query
    {
        public World World;

        public Mask Mask;

        int[] indices;
        Entity[] entities;
        int entityCount;

        int lockCount;
        DelayedOperation[] delayedOperations;
        int delayedOperationCount;

        public Query(World world, Mask mask, int entitySize)
        {
            World = world;
            Mask = mask;

            indices = new int[entitySize];
            entities = new Entity[entitySize];
            delayedOperations = new DelayedOperation[512];

            entityCount = 0;
            delayedOperationCount = 0;
            lockCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEntity(EntityId entityId)
        {
            if (AddDelayedOperation(entityId, true)) { return; }

            var index = entityCount++;

            if (entityId.Number >= indices.Length)
            {
                Array.Resize(ref indices, entityId.Number << 1);
            }

            indices[entityId.Number] = index;

            if (entityCount == entities.Length)
            {
                Array.Resize(ref entities, entityCount << 1);
            }

            entities[index] = new Entity(World, entityId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasEntity(EntityId entityId)
        {
            return indices.Length > entityId.Number && indices[entityId.Number] > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveEntity(EntityId entityId)
        {
            if (AddDelayedOperation(entityId, false)) { return; }

            var index = indices[entityId.Number];
            indices[entityId.Number] = 0;

            entityCount--;

            if (index < entityCount)
            {
                entities[index] = entities[entityCount];
                indices[entities[index].Id.Number] = index;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDelayedOperation(EntityId entityId, bool added)
        {
            if (lockCount <= 0) { return false; }

            if (delayedOperationCount == delayedOperations.Length)
            {
                Array.Resize(ref delayedOperations, delayedOperationCount << 1);
            }

            ref var op = ref delayedOperations[delayedOperationCount++];

            op.Added = added;
            op.EntityId = entityId;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Unlock()
        {
#if DEBUG
            if (lockCount <= 0)
            {
                throw new Exception($"Invalid lock-unlock balance for \"{GetType().Name}\".");
            }
#endif
            lockCount--;

            if (lockCount == 0 && delayedOperationCount > 0)
            {
                for (int i = 0; i < delayedOperationCount; i++)
                {
                    ref var op = ref delayedOperations[i];

                    if (op.Added)
                    {
                        AddEntity(op.EntityId);
                    }
                    else
                    {
                        RemoveEntity(op.EntityId);
                    }
                }

                delayedOperationCount = 0;
            }
        }

        public Enumerator GetEnumerator()
        {
            lockCount++;
            return new Enumerator(this);
        }

        public struct Enumerator : IDisposable
        {
            Query query;
            int index;

            public Enumerator(Query query)
            {
                this.query = query;
                index = -1;
            }

            public int Count { get { return query.entityCount; } }

            public Entity Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => query.entities[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                return ++index < query.entityCount;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                query.Unlock();
            }
        }

        struct DelayedOperation
        {
            public bool Added;
            public EntityId EntityId;
        }
    }

    public sealed class Mask
    {
        static readonly object syncObject = new object();
        static Mask[] maskPool = new Mask[32];
        static int maskPoolCount;

        internal static Mask New(World world)
        {
            lock (syncObject)
            {
                var mask = maskPoolCount > 0 ? maskPool[--maskPoolCount] : new Mask(world);
                mask.world = world;

                return mask;
            }
        }

        World world;

        internal BitSet IncludeBitSet;
        internal BitSet ExcludeBitSet;

        internal List<int> Types;

#if DEBUG
        bool isBuilt;
#endif

        public Mask(World world)
        {
            this.world = world;

            Types = new List<int>();

            IncludeBitSet = new BitSet();
            ExcludeBitSet = new BitSet();

            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Reset()
        {
            IncludeBitSet.ClearAll();
            ExcludeBitSet.ClearAll();

            Types.Clear();

#if DEBUG
            isBuilt = false;
#endif
        }

        public void With<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);

#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(index) != -1)
            {
                throw new Exception($"{typeof(T).Name} already in constraints list.");
            }
#endif

            IncludeBitSet.Set(index);
            Types.Add(index);
        }

        public void Without<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);

#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            if (Types.IndexOf(index) != -1)
            {
                throw new Exception($"{typeof(T).Name} already in constraints list.");
            }
#endif

            ExcludeBitSet.Set(index);
            Types.Add(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Query Apply(int capacity = 512)
        {
#if DEBUG
            if (isBuilt)
            {
                throw new Exception("Cant change built mask.");
            }

            isBuilt = true;
#endif      
            Types.Sort();

            var (query, isNew) = world.GetQuery(this, capacity);
            if (!isNew) { Recycle(); }
            return query;
        }

        private void Recycle()
        {
            Reset();

            lock (syncObject)
            {
                if (maskPoolCount == maskPool.Length)
                {
                    Array.Resize(ref maskPool, maskPoolCount << 1);
                }

                maskPool[maskPoolCount++] = this;
            }
        }

        public override int GetHashCode()
        {
            int hash = Types.Count;

            foreach (var index in Types)
            {
                hash = unchecked(hash * 314159 + index);
            }

            return hash;
        }
    }
}