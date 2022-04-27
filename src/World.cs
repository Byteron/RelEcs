using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public struct Element<T> where T : class
    {
        public T Value;
        public Element(T value) => Value = value;
    }

    public sealed class World
    {
        static int worldCounter;

        readonly Entity world;

        EntityId[] entities;
        BitSet[] bitsets;

        int entityCount;

        EntityId[] unusedIds;
        int unusedIdCount;

        Dictionary<long, int> storageIndices;
        IStorage[] storages;
        int storageCount;

        int relationCount;

        Dictionary<int, Query> hashedQueries;
        List<Query>[] queriesByTypeId;
        
        Dictionary<Type, Entity> targetTypeEntities = new Dictionary<Type, Entity>();
        
        internal List<(Type, TimeSpan)> SystemExecutionTimes;

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

            eventLifeTimeSystem = new EventLifeTimeSystem();

            AddElement(new WorldInfo(++worldCounter));
        }

        public Entity Spawn()
        {
            EntityId id;

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
                var targetTypeIndices = new List<int>();

                for (var i = 0; i <= storageCount; i++)
                {
                    if (storages[i] == null)
                    {
                        continue;
                    }

                    if (bitset.Get(i))
                    {
                        storages[i].Remove(id.Number);
                        bitset.Clear(i);
                        OnEntityChanged(id, i);
                    }

                    if (TypeId.Entity(storages[i].TypeId) == id.Number)
                    {
                        targetTypeIndices.Add(i);
                    }
                }

                if (bitset.Count != 0)
                {
                    throw new Exception("entity was despawned but still has components, something went wrong");
                }

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

                            if (!storage.Has(entity.Number)) continue;
                            
                            storage.Remove(entity.Number);
                            bitsets[entity.Number].Clear(index);
                            OnEntityChanged(entity, index);
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

            var eventStorage = GetStorage<T>(EntityId.None);
            var systemStorage = GetStorage<EventSystemList>(EntityId.None);

            var mask = new Mask();

            mask.Has(eventStorage.Index);
            mask.Has(systemStorage.Index);
            mask.Lock();

            var query = GetQuery(mask);


            foreach (var entity in query)
            {
                ref var systemList = ref systemStorage.Get(entity.Id.Number);

                if (systemList.List.Contains(systemType)) continue;
                
                systemList.List.Add(systemType);
                action(eventStorage.Get(entity.Id.Number));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddElement<T>(T element) where T : class
        {
            AddComponent<Element<T>>(world.Id) = new Element<T>(element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetElement<T>() where T : class
        {
            return GetComponent<Element<T>>(world.Id).Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetElement<T>(out T element) where T : class
        {
            if (HasElement<T>())
            {
                element = GetElement<T>();
                return true;
            }

            element = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasElement<T>() where T : class
        {
            return HasComponent<Element<T>>(world.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveElement<T>() where T : class
        {
            RemoveComponent<Element<T>>(world.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T AddComponent<T>(EntityId id, EntityId target = default, bool triggerEvent = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            bitsets[id.Number].Set(storage.Index);

            ref var component = ref storage.Add(id.Number);

            OnEntityChanged(id, storage.Index);
            
            if (triggerEvent)
            {
                Send(new Added<T>(new Entity(this, id)));
            }

            if (target != default)
            {
                relationCount++;
            }

            return ref component;
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

            return storageIndices.TryGetValue(typeId, out var index) && bitsets[id.Number].Get(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(EntityId id, EntityId target = default, bool triggerEvent = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            storage.Remove(id.Number);

            bitsets[id.Number].Clear(storage.Index);

            OnEntityChanged(id, storage.Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEntityChanged(EntityId entityId, int typeIndex)
        {
            var list = queriesByTypeId[typeIndex];

            if (list == null) return;
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
        
        public Entity GetTypeEntity<T>() where T : struct
        {
            if (targetTypeEntities.TryGetValue(typeof(T), out var entity) && entity.IsAlive)
            {
                return entity;
            }

            entity = Spawn();
            targetTypeEntities[typeof(T)] = entity;
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Query GetQuery(Mask mask)
        {
            var hash = mask.GetHashCode();

            if (hashedQueries.TryGetValue(hash, out var query))
            {
                return query;
            }

            query = new Query(this, mask, entityCount);
            hashedQueries[hash] = query;

            foreach (var typeIndex in mask.Types)
            {
                if (typeIndex >= queriesByTypeId.Length)
                {
                    Array.Resize(ref queriesByTypeId, typeIndex << 1);
                }

                ref var list = ref queriesByTypeId[typeIndex];

                if (list == null)
                {
                    list = new List<Query>(10);
                }

                list.Add(query);
            }

            for (var i = 0; i <= entityCount; i++)
            {
                var entityId = entities[i];

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

            return query;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsEntityCompatibleWithMask(Mask mask, EntityId entityId)
        {
            return !bitsets[entityId.Number].HasAnyBitSet(mask.NotBitSet)
            && bitsets[entityId.Number].HasAllBitsSet(mask.HasBitSet)
            && (mask.AnyBitSet.Count == 0 || bitsets[entityId.Number].HasAnyBitSet(mask.AnyBitSet));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick()
        {
            var info = GetElement<WorldInfo>();

            info.EntityCount = entityCount;
            info.UnusedEntityCount = unusedIdCount;
            info.AllocatedEntityCount = entities.Length;
            info.ComponentCount = storageCount;
            info.RelationCount = relationCount;
            info.ElementCount = bitsets[world.Id.Number].Count;
            info.SystemCount = SystemExecutionTimes.Count;
            info.CachedQueryCount = hashedQueries.Count;

            info.SystemExecutionTimes.Clear();
            info.SystemExecutionTimes.AddRange(SystemExecutionTimes);

            eventLifeTimeSystem.Run(this);

            SystemExecutionTimes.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(EntityId id)
        {
            return entities[id.Number] != EntityId.None;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IStorage GetStorage(int index)
        {
            return storages[index];
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
        public readonly int WorldId;
        public int EntityCount;
        public int UnusedEntityCount;
        public int AllocatedEntityCount;
        public int ComponentCount;
        public int RelationCount;
        public int ElementCount;
        public int SystemCount;
        public List<(Type, TimeSpan)> SystemExecutionTimes;
        public int CachedQueryCount;

        public WorldInfo(int id)
        {
            WorldId = id;
            SystemExecutionTimes = new List<(Type, TimeSpan)>();
        }
    }
}