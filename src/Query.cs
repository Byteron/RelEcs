using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class Query
    {
        readonly World world;

        public readonly Mask Mask;

        int[] indices;
        Entity[] entities;
        int entityCount;

        int lockCount;
        DelayedOperation[] delayedOperations;
        int delayedOperationCount;

        public Query(World world, Mask mask, int entitySize)
        {
            this.world = world;
            Mask = mask;

            indices = new int[entitySize];
            entities = new Entity[entitySize];
            delayedOperations = new DelayedOperation[512];

            entityCount = 0;
            delayedOperationCount = 0;
            lockCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEntity(Identity identity)
        {
            if (AddDelayedOperation(identity, true)) { return; }

            var index = ++entityCount;

            if (identity.Id >= indices.Length)
            {
                Array.Resize(ref indices, identity.Id << 1);
            }

            indices[identity.Id] = index;

            if (entityCount == entities.Length)
            {
                Array.Resize(ref entities, entityCount << 1);
            }

            entities[index] = new Entity(world, identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasEntity(Identity identity)
        {
            return indices.Length > identity.Id && indices[identity.Id] > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveEntity(Identity identity)
        {
            if (AddDelayedOperation(identity, false)) { return; }

            var index = indices[identity.Id];
            indices[identity.Id] = 0;

            if (index < entityCount)
            {
                entities[index] = entities[entityCount];
                indices[entities[index].Identity.Id] = index;
            }

            entityCount--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDelayedOperation(Identity identity, bool added)
        {
            if (lockCount <= 0) { return false; }

            if (delayedOperationCount == delayedOperations.Length)
            {
                Array.Resize(ref delayedOperations, delayedOperationCount << 1);
            }

            ref var op = ref delayedOperations[delayedOperationCount++];

            op.Added = added;
            op.Identity = identity;

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
                        AddEntity(op.Identity);
                    }
                    else
                    {
                        RemoveEntity(op.Identity);
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

        public int Count => entityCount;

        public struct Enumerator : IDisposable
        {
            readonly Query query;
            int index;

            public Enumerator(Query query)
            {
                this.query = query;
                index = 0;
            }

            public Entity Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => query.entities[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                return ++index <= query.entityCount;
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
            public Identity Identity;
        }
    }
}