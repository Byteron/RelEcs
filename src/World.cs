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

        int entityCount = 0;

        EntityId[] unusedIds = null;
        int unusedIdCount = 0;

        Dictionary<long, int> storageIndices = null;
        IStorage[] storages = null;
        int storageCount = 0;

        int eventLifeTimeIndex;
        EventLifeTimeSystem eventLifeTimeSystem;

        Config config;

        public World() : this(new Config()) { }

        public World(Config config)
        {
            Number = counter++;

            entities = new EntityId[config.EntitySize];
            bitsets = new BitSet[config.EntitySize];
            unusedIds = new EntityId[config.EntitySize];
            storageIndices = new Dictionary<long, int>(config.ComponentSize);
            storages = new IStorage[config.ComponentSize];

            this.config = config;

            world = Spawn();

            eventLifeTimeIndex = GetStorage<EventLifeTime>(EntityId.None).Index;
            eventLifeTimeSystem = new EventLifeTimeSystem();
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
                }

                entities[id.Number] = id;
            }

            bitsets[id.Number] ??= new BitSet();

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

            for (int i = 0; i < storages.Length; i++)
            {
                if (storages[i] == null)
                {
                    continue;
                }

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

            if (targetTypeIndices.Count > 0)
            {
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
            }

            if (unusedIdCount == unusedIds.Length)
            {
                Array.Resize(ref unusedIds, unusedIdCount << 1);
            }

            id.Generation++;
            unusedIds[unusedIdCount++] = id;
            entities[id.Number] = EntityId.None;
        }

        public void Send<T>(T eventStruct) where T : struct
        {
            Send<T>() = eventStruct;
        }

        public ref T Send<T>() where T : struct
        {
            var typeId = TypeId.Value<T>(0);

            var entity = Spawn();

            AddComponent<EventSystemList>(entity.Id);
            AddComponent<EventLifeTime>(entity.Id);

            return ref AddComponent<T>(entity.Id);
        }

        public void Receive<T>(ISystem system, Action<T> action) where T : struct
        {
            var systemType = system.GetType();
            
            var mask = new Mask(this);
            mask.With<T>(Entity.None);

            var query = GetQuery(mask);

            var eventStorage = GetStorage<T>(EntityId.None);
            var systemStorage = GetStorage<EventSystemList>(EntityId.None);

            foreach (var entity in query.Entities)
            {
                ref var systemList = ref systemStorage.Get(entity.Id.Number);

                if (systemList.List == null)
                {
                    systemList.List = new List<Type>();
                    systemList.List.Add(systemType);

                    action(eventStorage.Get(entity.Id.Number));
                }
                else if (!systemList.List.Contains(systemType))
                {
                    systemList.List.Add(systemType);

                    action(eventStorage.Get(entity.Id.Number));
                }
            }
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

        public ref T AddComponent<T>(EntityId id, EntityId target = default, bool triggerEvent = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            bitsets[id.Number].Set(storage.Index);

            if (triggerEvent)
            {
                Send(new Added<T>(new Entity(this, target)));
            }

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

        public void RemoveComponent<T>(EntityId id, EntityId target = default, bool triggerEvent = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            storage.Remove(id.Number);

            if (triggerEvent)
            {
                Send(new Removed<T>(new Entity(this, target)));
            }

            bitsets[id.Number].Clear(storage.Index);
        }

        public Query GetQuery(Mask mask)
        {
            var entities = this.entities
                .Where(id => IsAlive(id) && id != world.Id)
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

            index = storageCount++;
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

        public void Tick()
        {
            eventLifeTimeSystem.Run(this);
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