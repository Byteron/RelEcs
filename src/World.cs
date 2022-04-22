using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public struct Resource<T> where T : class
    {
        public T Value;
        public Resource(T value) => Value = value;
    }

    public sealed class World
    {
        static int worldCounter = 0;

        Entity world;
        int number;

        EntityId[] entities = null;
        BitSet[] bitsets = null;

        int entityCount = 0;

        EntityId[] unusedIds = null;
        int unusedIdCount = 0;

        Dictionary<long, int> storageIndices = null;
        IStorage[] storages = null;
        int storageCount = 0;

        int relationCount = 0;

        Dictionary<int, Query> hashedQueries;
        List<Query>[] queriesByTypeId;

        internal List<(Type, TimeSpan)> SystemExecutionTimes;

        int eventLifeTimeIndex;
        EventLifeTimeSystem eventLifeTimeSystem;

        WorldConfig config;

        public World() : this(new WorldConfig()) { }

        public World(WorldConfig config)
        {
            entities = new EntityId[config.EntitySize];
            bitsets = new BitSet[config.EntitySize];
            unusedIds = new EntityId[config.EntitySize];

            storageIndices = new Dictionary<long, int>(config.ComponentSize);
            storages = new IStorage[config.ComponentSize];

            hashedQueries = new Dictionary<int, Query>();
            queriesByTypeId = new List<Query>[config.ComponentSize];

            SystemExecutionTimes = new List<(Type, TimeSpan)>();

            this.config = config;

            world = Spawn();
            number = ++worldCounter;

            eventLifeTimeIndex = GetStorage<EventLifeTime>(EntityId.None).Index;
            eventLifeTimeSystem = new EventLifeTimeSystem();

            var info = new WorldInfo()
            {
                WorldId = number,
                SystemExecutionTimes = new List<(Type, TimeSpan)>(),
            };

            AddResource(info);
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

            if (bitsets[id.Number] == null)
            {
                bitsets[id.Number] = new BitSet();
            }

            return new Entity(this, id);
        }

        public void Despawn(EntityId id)
        {
            if (!IsAlive(entities[id.Number]))
            {
                return;
            }

            var bitset = bitsets[id.Number];

            if (bitset.Count > 0)
            {
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
                        OnEntityChanged(id, i);
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
                                OnEntityChanged(entity, index);
                            }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send<T>(T eventStruct) where T : struct
        {
            Send<T>() = eventStruct;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Send<T>() where T : struct
        {
            var entity = Spawn();

            AddComponent<EventSystemList>(entity.Id);
            AddComponent<EventLifeTime>(entity.Id);

            return ref AddComponent<T>(entity.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Receive<T>(ISystem system, Action<T> action) where T : struct
        {
            var systemType = system.GetType();

            var mask = Mask.New(this);
            mask.With<T>(Entity.None);
            var query = mask.Apply();

            var eventStorage = GetStorage<T>(EntityId.None);
            var systemStorage = GetStorage<EventSystemList>(EntityId.None);

            foreach (var entity in query)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddResource<T>(T resource) where T : class
        {
            AddComponent<Resource<T>>(world.Id) = new Resource<T>(resource);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetResource<T>() where T : class
        {
            return GetComponent<Resource<T>>(world.Id).Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetResource<T>(out T resource) where T : class
        {
            if (HasResource<T>())
            {
                resource = GetResource<T>();
                return true;
            }

            resource = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasResource<T>() where T : class
        {
            return HasComponent<Resource<T>>(world.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveResource<T>() where T : class
        {
            RemoveComponent<Resource<T>>(world.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T AddComponent<T>(EntityId id, EntityId target = default, bool triggerEvent = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            bitsets[id.Number].Set(storage.Index);

            if (triggerEvent)
            {
                Send(new Added<T>(new Entity(this, target)));
            }

            if (target != default)
            {
                relationCount++;
            }

            OnEntityChanged(id, storage.Index);

            return ref storage.Add(id.Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetComponent<T>(EntityId id, EntityId target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            return ref storage.Get(id.Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<T>(EntityId id, EntityId target = default) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Number);

            if (storageIndices.TryGetValue(typeId, out var index))
            {
                return bitsets[id.Number].Get(index);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(EntityId id, EntityId target = default, bool triggerEvent = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            storage.Remove(id.Number);

            if (triggerEvent)
            {
                Send(new Removed<T>(new Entity(this, target)));
            }

            bitsets[id.Number].Clear(storage.Index);

            OnEntityChanged(id, storage.Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEntityChanged(EntityId entityId, int typeId)
        {
            var list = queriesByTypeId[typeId];

            if (list != null)
            {
                foreach (var query in list)
                {
                    var isCompatible = IsEntityCompatibleWithMask(query.Mask, entityId);
                    var isInQuery = query.HasEntity(entityId);

                    if (isCompatible && !isInQuery)
                    {
                        query.AddEntity(entityId);
                    }
                    else if (!isCompatible && isInQuery)
                    {
                        query.RemoveEntity(entityId);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (Query, bool) GetQuery(Mask mask, int capacity)
        {
            var hash = mask.GetHashCode();

            if (hashedQueries.TryGetValue(hash, out var query))
            {
                return (query, false);
            }

            query = new Query(this, mask, entityCount);
            hashedQueries[hash] = query;

            foreach (var typeId in mask.Types)
            {
                ref var list = ref queriesByTypeId[typeId];

                if (list == null)
                {
                    list = new List<Query>(10);
                }

                list.Add(query);
            }

            for (int i = 0; i <= entityCount; i++)
            {
                var entityId = this.entities[i];

                if (!IsAlive(entityId) || entityId == world.Id)
                {
                    continue;
                }

                if (!IsEntityCompatibleWithMask(mask, entityId))
                {
                    continue;
                }

                query.AddEntity(entityId);
            }

            return (query, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEntityCompatibleWithMask(Mask mask, EntityId entityId)
        {
            return !bitsets[entityId.Number].HasAnyBitSet(mask.ExcludeBitSet)
            && bitsets[entityId.Number].HasAllBitsSet(mask.IncludeBitSet);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IStorage GetStorage(int index)
        {
            return storages[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick()
        {
            var info = GetResource<WorldInfo>();

            info.EntityCount = entityCount;
            info.UnusedEntityCount = unusedIdCount;
            info.AllocatedEntityCount = entities.Length;
            info.ComponentCount = storageCount;
            info.RelationCount = relationCount;
            info.ResourceCount = bitsets[world.Id.Number].Count;
            info.SystemCount = SystemExecutionTimes.Count;
            info.CachedQueryCount = hashedQueries.Count;

            info.SystemExecutionTimes.Clear();
            info.SystemExecutionTimes.AddRange(SystemExecutionTimes);

            eventLifeTimeSystem.Run(this);

            SystemExecutionTimes.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetStorageIndex(long typeId)
        {
            return storageIndices.TryGetValue(typeId, out var index) ? index : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(EntityId id)
        {
            return entities[id.Number] != EntityId.None;
        }
    }

    public sealed class WorldConfig
    {
        public int EntitySize = 32;
        public int StorageSize = 32;
        public int ComponentSize = 32;
    }

    public sealed class WorldInfo
    {
        public int WorldId;
        public int EntityCount;
        public int UnusedEntityCount;
        public int AllocatedEntityCount;
        public int ComponentCount;
        public int RelationCount;
        public int ResourceCount;
        public int SystemCount;
        public List<(Type, TimeSpan)> SystemExecutionTimes;
        public int CachedQueryCount;
    }
}