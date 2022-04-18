using System;
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

        Id[] entities = new Id[512];
        BitSet[] bitsets = new BitSet[512];

        int entityCount = 0;

        Id[] unusedIds = new Id[512];
        int unusedIdCount = 0;

        IStorage[] storages = new IStorage[512];

        public World()
        {
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
                }

                entities[id.Number] = id;
            }

            ref var bitset = ref bitsets[id.Number];

            if (bitset == null)
            {
                bitset = new BitSet();
            }

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
            var targetIndex = storage.TypeId.Index;
            var sourceIndex = TypeId.Get<T>(id).Index;

            var bitset = bitsets[id.Number];
            bitset.Set(targetIndex);

            return ref storage.Add(id.Number);
        }

        public ref T GetComponent<T>(Id id, Id target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            var index = storage.TypeId.Index;

            return ref storage.Get(id.Number);
        }

        public bool HasComponent<T>(Id id, Id target = default) where T : struct
        {
            var typeId = TypeId.Get<T>(target);
            var bitset = bitsets[id.Number];
            return bitset.Get(typeId.Index);
        }

        public void RemoveComponent<T>(Id id, Id target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            storage.Remove(id.Number);

            var bitset = bitsets[id.Number];
            bitset.Clear(storage.TypeId.Index);
        }

        internal Entity[] Query(Mask mask)
        {
            List<Entity> filteredEntities = new List<Entity>();

            for (var i = 1; i <= entityCount; i++)
            {
                Id id = entities[i];

                if (!IsAlive(id))
                {
                    continue;
                }

                var bitset = bitsets[i];

                if (!bitset.HasAllBitsSet(mask.IncludeBitSet))
                {
                    continue;
                }

                if (bitset.HasAnyBitSet(mask.ExcludeBitSet))
                {
                    continue;
                }

                filteredEntities.Add(new Entity(this, id));
            }

            return filteredEntities.ToArray();
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
                storages[typeId.Index] = new Storage<T>(typeId);
            }

            storage = storages[typeId.Index] as Storage<T>;

            return storage;
        }

        public IStorage GetStorage(TypeId typeId)
        {
            return storages[typeId.Index];
        }

        public bool IsAlive(Id id)
        {
            return entities[id.Number] != Id.None;
        }
    }
}