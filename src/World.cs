using System;
using System.Linq;
using System.Collections.Generic;
namespace Bitron.Ecs
{
    internal struct Resource<T> where T : class
    {
        internal T Value;
        internal Resource(T value) => Value = value;
    }

    public sealed class World
    {
        Entity world;

        Id[] entities = null;
        BitSet[] bitsets = null;
        BitSet[] addedBitsets = null;
        BitSet[] removedBitsets = null;

        int entityCount = 0;

        Id[] unusedIds = null;
        int unusedIdCount = 0;

        IStorage[] storages = null;

        Config config;

        public World() : this(new Config()) { }

        public World(Config config)
        {
            entities = new Id[config.EntitySize];
            bitsets = new BitSet[config.EntitySize];
            addedBitsets = new BitSet[config.EntitySize];
            removedBitsets = new BitSet[config.EntitySize];
            unusedIds = new Id[config.EntitySize];
            storages = new IStorage[config.ComponentSize];
            this.config = config;
            world = Spawn();
        }

        public Entity Spawn()
        {
            Id id = default;

            if (unusedIdCount > 0)
            {
                id = unusedIds[--unusedIdCount];
            }
            else
            {
                id = new Id(++entityCount, 1);

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

        public void Despawn(Id id)
        {
            if (!IsAlive(entities[id.Number]))
            {
                return;
            }

            var bitset = bitsets[id.Number];

            List<TypeId> targetTypes = new List<TypeId>();

            for (int i = 0; i < storages.Length; i++)
            {
                if (storages[i] == null)
                {
                    continue;
                }

                var typeId = storages[i].TypeId;

                if (bitset.Get(typeId.Index))
                {
                    storages[i].Remove(id.Number);
                }

                if (typeId.Entity == id.Number)
                {
                    targetTypes.Add(typeId);
                }
            }

            bitset.ClearAll();

            foreach (var entity in entities)
            {
                if (!IsAlive(entity))
                {
                    continue;
                }

                foreach (var typeId in targetTypes)
                {
                    var storage = GetStorage(typeId);

                    if (storage.Has(entity.Number))
                    {
                        storage.Remove(entity.Number);
                        bitsets[entity.Number].Clear(typeId.Index);
                    }
                }
            }

            if (unusedIdCount == unusedIds.Length)
            {
                Array.Resize(ref unusedIds, unusedIdCount << 1);
            }

            id.Generation++;
            unusedIds[unusedIdCount++] = id;
            entities[id.Number] = Id.None;
        }

        public void AddResource<T>(T resource) where T : class
        {
            AddComponent<Resource<T>>(world.Id);
        }

        public T GetResource<T>() where T : class
        {
            return GetComponent<Resource<T>>(world.Id).Value;
        }

        public void RemoveResource<T>() where T : class
        {
            RemoveComponent<Resource<T>>(world.Id);
        }

        public ref T AddComponent<T>(Id id, Id target = default) where T : struct
        {
            var storage = GetStorage<T>(target);

            bitsets[id.Number].Set(storage.TypeId.Index);
            addedBitsets[id.Number].Set(storage.TypeId.Index);
            removedBitsets[id.Number].Clear(storage.TypeId.Index);

            return ref storage.Add(id.Number);
        }

        public ref T GetComponent<T>(Id id, Id target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            return ref storage.Get(id.Number);
        }

        public bool HasComponent<T>(Id id, Id target = default) where T : struct
        {
            var typeId = TypeId.Get<T>(target);
            return bitsets[id.Number].Get(typeId.Index);
        }

        public void RemoveComponent<T>(Id id, Id target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            storage.Remove(id.Number);

            bitsets[id.Number].Clear(storage.TypeId.Index);
            addedBitsets[id.Number].Clear(storage.TypeId.Index);
            removedBitsets[id.Number].Set(storage.TypeId.Index);
        }

        internal Entity[] Query(Mask mask)
        {
            return entities
                .Where(id => IsAlive(id) && id != world.Id)
                .Where(id => addedBitsets[id.Number].HasAllBitsSet(mask.AddedBitSet))
                .Where(id => removedBitsets[id.Number].HasAllBitsSet(mask.RemovedBitSet))
                .Where(id => !bitsets[id.Number].HasAnyBitSet(mask.ExcludeBitSet))
                .Where(id => bitsets[id.Number].HasAllBitsSet(mask.IncludeBitSet))
                .Select(id => new Entity(this, id))
                .ToArray();
        }

        public Storage<T> GetStorage<T>(Id target) where T : struct
        {
            Storage<T> storage = null;

            var typeId = TypeId.Get<T>(target);

            if (typeId.Index >= storages.Length)
            {
                Array.Resize(ref storages, (typeId.Index << 1));
            }

            if (storages[typeId.Index] == null)
            {
                storages[typeId.Index] = new Storage<T>(config, typeId);
            }

            storage = storages[typeId.Index] as Storage<T>;

            return storage;
        }

        public IStorage GetStorage(TypeId typeId)
        {
            return storages[typeId.Index];
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

        public bool IsAlive(Id id)
        {
            return entities[id.Number] != Id.None;
        }

        public sealed class Config
        {
            public int EntitySize = 32;
            public int StorageSize = 32;
            public int ComponentSize = 32;
        }
    }
}