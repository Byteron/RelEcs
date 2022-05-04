using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public struct Trigger<T> where T : struct
    {
        public T Value;
        public Trigger(T value) => Value = value;
    }
    
    public struct Element<T> where T : class
    {
        public T Value;
        public Element(T value) => Value = value;
    }

    public sealed class World
    {
        static int worldCounter;

        readonly Entity world;

        Identity[] entities;
        BitSet[] bitsets;

        int entityCount;

        Identity[] unusedIds;
        int unusedIdCount;

        Dictionary<long, int> storageIndices;
        IStorage[] storages;
        int storageCount;

        int relationCount;

        Dictionary<int, Query> hashedQueries;
        List<Query>[] queriesByTypeId;
        
        Dictionary<Type, Entity> targetTypeEntities = new Dictionary<Type, Entity>();
        
        internal List<(Type, TimeSpan)> SystemExecutionTimes;

        TriggerLifeTimeSystem triggerLifeTimeSystem;

        WorldConfig config;

        public World() : this(new WorldConfig()) { }

        public World(WorldConfig config)
        {
            entities = new Identity[config.EntitySize];
            bitsets = new BitSet[config.EntitySize];
            unusedIds = new Identity[config.EntitySize];

            storageIndices = new Dictionary<long, int>(config.ComponentSize);
            storages = new IStorage[config.ComponentSize];

            hashedQueries = new Dictionary<int, Query>();
            queriesByTypeId = new List<Query>[config.ComponentSize];

            SystemExecutionTimes = new List<(Type, TimeSpan)>();

            this.config = config;

            world = Spawn();

            triggerLifeTimeSystem = new TriggerLifeTimeSystem();

            AddElement(new WorldInfo(++worldCounter));
        }

        public Entity Spawn()
        {
            Identity id;

            if (unusedIdCount > 0)
            {
                id = unusedIds[--unusedIdCount];
            }
            else
            {
                id = new Identity(++entityCount, 1);

                if (entities.Length == entityCount)
                {
                    Array.Resize(ref entities, entityCount << 1);
                    Array.Resize(ref bitsets, entityCount << 1);
                    
                    for (var i = 1; i <= storageCount; i++)
                    {
                        storages[i].Resize(entityCount << 1);
                    }
                }

                entities[id.Id] = id;
            }

            if (bitsets[id.Id] == null)
            {
                bitsets[id.Id] = new BitSet();
            }

            return new Entity(this, id);
        }

        public void Despawn(Identity id)
        {
            if (!IsAlive(entities[id.Id]))
            {
                return;
            }

            var bitset = bitsets[id.Id];

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
                        storages[i].Remove(id.Id);
                        bitset.Clear(i);
                        OnEntityChanged(id, i);
                    }

                    if (TypeId.Entity(storages[i].TypeId) == id.Id)
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

                            if (!storage.Has(entity.Id)) continue;
                            
                            storage.Remove(entity.Id);
                            bitsets[entity.Id].Clear(index);
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
            entities[id.Id] = Identity.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send<T>(T triggerStruct) where T : struct
        {
            Send<T>() = new Trigger<T>(triggerStruct);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Trigger<T> Send<T>() where T : struct
        {
            var entity = Spawn();

            AddComponent<TriggerSystemList>(entity.Identity);
            AddComponent<TriggerLifeTime>(entity.Identity);

            return ref AddComponent<Trigger<T>>(entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Receive<T>(ISystem system, Action<T> action) where T : struct
        {
            var systemType = system.GetType();

            var triggerStorage = GetStorage<Trigger<T>>(Identity.None);
            var systemStorage = GetStorage<TriggerSystemList>(Identity.None);

            var mask = new Mask();

            mask.Has(triggerStorage.Index);
            mask.Has(systemStorage.Index);
            mask.Lock();

            var query = GetQuery(mask);


            foreach (var entity in query)
            {
                ref var systemList = ref systemStorage.Get(entity.Identity.Id);

                if (systemList.List.Contains(systemType)) continue;
                
                systemList.List.Add(systemType);
                action(triggerStorage.Get(entity.Identity.Id).Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddElement<T>(T element) where T : class
        {
            AddComponent<Element<T>>(world.Identity) = new Element<T>(element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetElement<T>() where T : class
        {
            return GetComponent<Element<T>>(world.Identity).Value;
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
            return HasComponent<Element<T>>(world.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveElement<T>() where T : class
        {
            RemoveComponent<Element<T>>(world.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T AddComponent<T>(Identity id, Identity target = default, bool spawnTrigger = false) where T : struct
        {
            var storage = GetStorage<T>(target);

            bitsets[id.Id].Set(storage.Index);

            ref var component = ref storage.Add(id.Id);

            OnEntityChanged(id, storage.Index);
            
            if (spawnTrigger)
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
        public ref T GetComponent<T>(Identity id, Identity target = default) where T : struct
        {
            var storage = GetStorage<T>(target);
            return ref storage.Get(id.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<T>(Identity id, Identity target = default) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id);

            return storageIndices.TryGetValue(typeId, out var index) && bitsets[id.Id].Get(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(Identity id, Identity target = default) where T : struct
        {
            var storage = GetStorage<T>(target);

            storage.Remove(id.Id);

            bitsets[id.Id].Clear(storage.Index);

            OnEntityChanged(id, storage.Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEntityChanged(Identity identity, int typeIndex)
        {
            var list = queriesByTypeId[typeIndex];

            if (list == null) return;
            foreach (var query in list)
            {
                var isCompatible = IsEntityCompatibleWithMask(query.Mask, identity);
                var isInQuery = query.HasEntity(identity);

                if (isCompatible && !isInQuery)
                {
                    query.AddEntity(identity);
                }
                else if (!isCompatible && isInQuery)
                {
                    query.RemoveEntity(identity);
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
                var identity = entities[i];

                if (!IsAlive(identity) || identity == world.Identity)
                {
                    continue;
                }

                if (!IsEntityCompatibleWithMask(mask, identity))
                {
                    continue;
                }

                query.AddEntity(identity);
            }

            return query;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsEntityCompatibleWithMask(Mask mask, Identity identity)
        {
            return !bitsets[identity.Id].HasAnyBitSet(mask.NotBitSet)
            && bitsets[identity.Id].HasAllBitsSet(mask.HasBitSet)
            && (mask.AnyBitSet.Count == 0 || bitsets[identity.Id].HasAnyBitSet(mask.AnyBitSet));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Storage<T> GetStorage<T>(Identity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id);

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

            var storage = new Storage<T>(config, index, typeId, entityCount << 1);

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
            info.ElementCount = bitsets[world.Identity.Id].Count;
            info.SystemCount = SystemExecutionTimes.Count;
            info.CachedQueryCount = hashedQueries.Count;

            info.SystemExecutionTimes.Clear();
            info.SystemExecutionTimes.AddRange(SystemExecutionTimes);

            triggerLifeTimeSystem.Run(this);

            SystemExecutionTimes.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(Identity id)
        {
            return entities[id.Id] != Identity.None;
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