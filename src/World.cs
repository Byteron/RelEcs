using System;
using System.Linq;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public struct Resource<T> where T : class
    {
        public T Value;
        public Resource(T value) => Value = value;
    }

    public sealed class World
    {
        private static int counter = 0;

        public int Number;

        Entity world;

        EntityId[] entities = null;
        BitSet[] bitsets = null;
        BitSet[] addedBitsets = null;
        BitSet[] removedBitsets = null;

        int entityCount = 0;

        EntityId[] unusedIds = null;
        int unusedIdCount = 0;

        Dictionary<long, int> storageIndices = null;
        IStorage[] storages = null;
        int storageCount = 0;

        Config config;

        public World() : this(new Config()) { }

        public World(Config config)
        {
            Number = counter++;

            entities = new EntityId[config.EntitySize];
            bitsets = new BitSet[config.EntitySize];
            addedBitsets = new BitSet[config.EntitySize];
            removedBitsets = new BitSet[config.EntitySize];
            unusedIds = new EntityId[config.EntitySize];
            storageIndices = new Dictionary<long, int>(config.ComponentSize);
            storages = new IStorage[config.ComponentSize];

            this.config = config;

            world = Spawn();
        }

        public Entity Spawn()
        {
            EntityId id = default;

            if (unusedIdCount > 0)
            {
                id = unusedIds[--unusedIdCount];
            }
            else
            {
                id = new EntityId(++entityCount, 1);

                if (entities.Length == entityCount)
                {
                    Array.Resize(ref entities, entityCount << 1);
                    Array.Resize(ref bitsets, entityCount << 1);
                    Array.Resize(ref addedBitsets, entityCount << 1);
                    Array.Resize(ref removedBitsets, entityCount << 1);
                }

                entities[id.Number] = id;
            }

            bitsets[id.Number] ??= new BitSet();
            addedBitsets[id.Number] ??= new BitSet();
            removedBitsets[id.Number] ??= new BitSet();

            return new Entity(this, id);
        }

        public void Despawn(EntityId id)
        {
            if (!IsAlive(entities[id.Number]))
            {
                return;
            }

            var bitset = bitsets[id.Number];

            List<int> targetTypeIndices = new List<int>();

            for (int i = 1; i < storages.Length; i++)
            {
                var typeId = storages[i].TypeId;

                if (bitset.Get(i))
                {
                    storages[i].Remove(id.Number);
                }

                if (TypeId.Entity(typeId) == id.Number)
                {
                    targetTypeIndices.Add(i);
                }
            }

            bitset.ClearAll();

            foreach (var entity in entities)
            {
                if (!IsAlive(entity))
                {
                    continue;
                }

                foreach (var index in targetTypeIndices)
                {
                    var storage = GetStorage(index);

                    if (storage.Has(entity.Number))
                    {
                        storage.Remove(entity.Number);
                        bitsets[entity.Number].Clear(index);
                    }
                }
            }

            if (unusedIdCount == unusedIds.Length)
            {
                Array.Resize(ref unusedIds, unusedIdCount << 1);
            }

            id.Generation++;
            unusedIds[unusedIdCount++] = id;
            entities[id.Number] = EntityId.None;
        }

        public void AddResource<T>(T resource) where T : class
        {
            AddComponent<Resource<T>>(world.Id) = new Resource<T>(resource);
        }

        public T GetResource<T>() where T : class
        {
            return GetComponent<Resource<T>>(world.Id).Value;
        }

        public void RemoveResource<T>() where T : class
        {
            RemoveComponent<Resource<T>>(world.Id);
        }

        public ref T AddComponent<T>(EntityId id, EntityId target = default) where T : struct
        {
            var storage = GetStorage<T>(target);

            bitsets[id.Number].Set(storage.Index);
            addedBitsets[id.Number].Set(storage.Index);
            removedBitsets[id.Number].Clear(storage.Index);

            return ref storage.Add(id.Number);
        }

        public ref T GetComponent<T>(EntityId id, EntityId target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            return ref storage.Get(id.Number);
        }

        public bool HasComponent<T>(EntityId id, EntityId target = default) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Number);

            if (storageIndices.TryGetValue(typeId, out var index))
            {
                return bitsets[id.Number].Get(index);
            }

            return false;
        }

        public void RemoveComponent<T>(EntityId id, EntityId target = default) where T : struct
        {
            var storage = GetStorage<T>(target);

            storage.Remove(id.Number);

            bitsets[id.Number].Clear(storage.Index);
            addedBitsets[id.Number].Clear(storage.Index);
            removedBitsets[id.Number].Set(storage.Index);
        }

        public Query GetQuery(Mask mask)
        {
            var entities = this.entities
                .Where(id => IsAlive(id) && id != world.Id)
                .Where(id => addedBitsets[id.Number].HasAllBitsSet(mask.AddedBitSet))
                .Where(id => removedBitsets[id.Number].HasAllBitsSet(mask.RemovedBitSet))
                .Where(id => !bitsets[id.Number].HasAnyBitSet(mask.ExcludeBitSet))
                .Where(id => bitsets[id.Number].HasAllBitsSet(mask.IncludeBitSet))
                .Select(id => new Entity(this, id))
                .ToArray();

            return new Query(this, mask, entities);
        }

        public Storage<T> GetStorage<T>(EntityId target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Number);

            if (storageIndices.TryGetValue(typeId, out var index))
            {
                return storages[index] as Storage<T>;
            }

            index = ++storageCount;
            storageIndices[typeId] = index;

            if (index >= storages.Length)
            {
                Array.Resize(ref storages, (index << 1));
            }

            var storage = new Storage<T>(config, index, typeId);

            storages[index] = storage;

            return storage;
        }

        public IStorage GetStorage(int index)
        {
            return storages[index];
        }

        public void Cleanup()
        {
            foreach (var entity in entities)
            {
                if (!IsAlive(entity))
                {
                    continue;
                }

                addedBitsets[entity.Number].ClearAll();
                removedBitsets[entity.Number].ClearAll();
            }
        }

        public int GetStorageIndex(long typeId)
        {
            return storageIndices.TryGetValue(typeId, out var index) ? index : 0;
        }

        public bool IsAlive(EntityId id)
        {
            return entities[id.Number] != EntityId.None;
        }

        public sealed class Config
        {
            public int EntitySize = 32;
            public int StorageSize = 32;
            public int ComponentSize = 32;
        }
    }
}