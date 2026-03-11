using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class World
    {
        static int worldCount;

        readonly Entity _world;
        readonly WorldInfo _worldInfo;

        readonly Archetypes _archetypes = new();

        readonly TriggerLifeTimeSystem _triggerLifeTimeSystem = new();
        readonly Dictionary<StorageType, List<ITriggerSystem>> _triggerSystems = new();
        private List<Action> _triggerSystemsToRun = new();

        public WorldInfo Info => _worldInfo;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public World()
        {
            _world = _archetypes.Spawn();
            _worldInfo = new WorldInfo(++worldCount);
            _archetypes.AddComponent(StorageType.Create<WorldInfo>(Identity.None), _world.Identity, _worldInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EntityBuilder Spawn()
        {
            return new EntityBuilder(this, _archetypes.Spawn());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EntityBuilder On(Entity entity)
        {
            return new EntityBuilder(this, entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Despawn(Entity entity)
        {
            _archetypes.Despawn(entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DespawnAllWith<T>() where T : class
        {
            var query = Query<Entity>().Has<T>().Build();
            foreach (var entity in query) Despawn(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(Entity? entity)
        {
            return entity is not null && _archetypes.IsAlive(entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>(Entity entity) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            return (T)_archetypes.GetComponent(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(Entity entity, out T? component) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            if (!HasComponent<T>(entity))
            {
                component = null;
                return false;
            }

            component = (T)_archetypes.GetComponent(type, entity.Identity);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<T>(Entity entity) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            return _archetypes.HasComponent(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(Entity entity) where T : class, new()
        {
            var type = StorageType.Create<T>(Identity.None);
            _archetypes.AddComponent(type, entity.Identity, new T());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(Entity entity, T component) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            _archetypes.AddComponent(type, entity.Identity, component);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(Entity entity) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            _archetypes.RemoveComponent(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<(StorageType, object?)> GetComponents(Entity entity)
        {
            return _archetypes.GetComponents(entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>(Entity entity, Entity target) where T : class
        {
            var type = StorageType.Create<T>(target.Identity);
            return (T)_archetypes.GetComponent(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(Entity entity, out T? component, Entity target) where T : class
        {
            var type = StorageType.Create<T>(target.Identity);
            if (!HasComponent<T>(entity))
            {
                component = null;
                return false;
            }

            component = (T)_archetypes.GetComponent(type, entity.Identity);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<T>(Entity entity, Entity target) where T : class
        {
            var type = StorageType.Create<T>(target.Identity);
            return _archetypes.HasComponent(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(Entity entity, Entity target) where T : class, new()
        {
            var type = StorageType.Create<T>(target.Identity);
            _archetypes.AddComponent(type, entity.Identity, new T());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(Entity entity, T component, Entity target) where T : class
        {
            var type = StorageType.Create<T>(target.Identity);
            _archetypes.AddComponent(type, entity.Identity, component);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(Entity entity, Entity target) where T : class
        {
            var type = StorageType.Create<T>(target.Identity);
            _archetypes.RemoveComponent(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity GetTarget<T>(Entity entity) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            return _archetypes.GetTarget(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<Entity> GetTargets<T>(Entity entity) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            return _archetypes.GetTargets(type, entity.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetElement<T>() where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            return (T)_archetypes.GetComponent(type, _world.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetElement<T>(out T? element) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            if (!HasElement<T>())
            {
                element = null;
                return false;
            }

            element = (T)_archetypes.GetComponent(type, _world.Identity);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasElement<T>() where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            return _archetypes.HasComponent(type, _world.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddElement<T>(T element) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            _archetypes.AddComponent(type, _world.Identity, element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReplaceElement<T>(T element) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            _archetypes.RemoveComponent(type, _world.Identity);
            _archetypes.AddComponent(type, _world.Identity, element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddOrReplaceElement<T>(T element) where T : class
        {
            var type = StorageType.Create<T>(Identity.None);

            if (_archetypes.HasComponent(type, _world.Identity))
            {
                _archetypes.RemoveComponent(type, _world.Identity);
            }

            _archetypes.AddComponent(type, _world.Identity, element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveElement<T>() where T : class
        {
            var type = StorageType.Create<T>(Identity.None);
            _archetypes.RemoveComponent(type, _world.Identity);
        }

        public void RegisterTriggerSystem<T>(ITriggerSystem<T> system)
        {
            var storageType = StorageType.Create<Trigger<T>>();
            
            if (!_triggerSystems.TryGetValue(storageType, out var systems))
            {
                _triggerSystems[storageType] = new List<ITriggerSystem>();
            }
            
            _triggerSystems[storageType].Add(system);
        }

        public void RunTriggerSystems()
        {
            foreach (var triggerSystem in _triggerSystemsToRun)
            {
                triggerSystem();
            }
            
            _triggerSystemsToRun.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send<T>(T trigger) where T : class
        {
            if (trigger is null) throw new Exception("trigger cannot be null");

            var entity = _archetypes.Spawn();
            _archetypes.AddComponent(StorageType.Create<SystemList>(), entity.Identity, new SystemList());
            _archetypes.AddComponent(StorageType.Create<LifeTime>(), entity.Identity, new LifeTime());
            _archetypes.AddComponent(StorageType.Create<Trigger<T>>(), entity.Identity, new Trigger<T> { Value = trigger });

            if (!_triggerSystems.TryGetValue(StorageType.Create<Trigger<T>>(), out var systems)) return;

            foreach (var system in systems)
            {
                var typedSystem = system as ITriggerSystem<T>;
                _triggerSystemsToRun.Add(() => typedSystem!.Run(this, trigger));
            }
        }
        
        // TODO: maybe move this into _archetypes
        readonly Dictionary<Type, Dictionary<int, Query>> _triggerQueries = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TriggerQuery<T> Receive<T>(ISystem system) where T : class
        {
            var mask = MaskPool.Get();

            mask.Has(StorageType.Create<Trigger<T>>(Identity.None));

            var hash = mask.GetHashCode();

            if (!_triggerQueries.TryGetValue(system.GetType(), out var dict))
            {
                dict = new Dictionary<int, Query>();
                _triggerQueries.Add(system.GetType(), dict);
            }

            if (dict.TryGetValue(hash, out var query))
            {
                MaskPool.Add(mask);
                return (TriggerQuery<T>)query;
            }
            
            // TODO: This is kind of hacky. Figure out a better way to make sure trigger queries have the right tables
            var dummy = _archetypes.Spawn();
            _archetypes.AddComponent(StorageType.Create<SystemList>(), dummy.Identity, new SystemList());
            _archetypes.AddComponent(StorageType.Create<LifeTime>(), dummy.Identity, new LifeTime());
            _archetypes.AddComponent(StorageType.Create<Trigger<T>>(), dummy.Identity, new Trigger<T> { Value = default! });
            _archetypes.Despawn(dummy.Identity);

            var matchingTables = new List<Table>();

            var type = mask.HasTypes[0];
            if (!_archetypes.TablesByType.TryGetValue(type, out var typeTables))
            {
                typeTables = new List<Table>();
                _archetypes.TablesByType[type] = typeTables;
            }

            foreach (var table in typeTables)
            {
                if (!_archetypes.IsMaskCompatibleWith(mask, table)) continue;

                matchingTables.Add(table);
            }

            query = new TriggerQuery<T>(_archetypes, mask, matchingTables, system.GetType());
            dict.Add(hash, query);

            return (TriggerQuery<T>)query;
        }

        public QueryBuilder<Entity> Query()
        {
            return new QueryBuilder<Entity>(_archetypes);
        }

        public QueryBuilder<C> Query<C>() where C : class
        {
            return new QueryBuilder<C>(_archetypes);
        }

        public QueryBuilder<C1, C2> Query<C1, C2>() where C1 : class where C2 : class
        {
            return new QueryBuilder<C1, C2>(_archetypes);
        }

        public QueryBuilder<C1, C2, C3> Query<C1, C2, C3>() where C1 : class where C2 : class where C3 : class
        {
            return new QueryBuilder<C1, C2, C3>(_archetypes);
        }

        public QueryBuilder<C1, C2, C3, C4> Query<C1, C2, C3, C4>() where C1 : class
            where C2 : class
            where C3 : class
            where C4 : class
        {
            return new QueryBuilder<C1, C2, C3, C4>(_archetypes);
        }

        public QueryBuilder<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>() where C1 : class
            where C2 : class
            where C3 : class
            where C4 : class
            where C5 : class
        {
            return new QueryBuilder<C1, C2, C3, C4, C5>(_archetypes);
        }

        public QueryBuilder<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>() where C1 : class
            where C2 : class
            where C3 : class
            where C4 : class
            where C5 : class
            where C6 : class
        {
            return new QueryBuilder<C1, C2, C3, C4, C5, C6>(_archetypes);
        }

        public QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>() where C1 : class
            where C2 : class
            where C3 : class
            where C4 : class
            where C5 : class
            where C6 : class
            where C7 : class
        {
            return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7>(_archetypes);
        }

        public QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>()
            where C1 : class
            where C2 : class
            where C3 : class
            where C4 : class
            where C5 : class
            where C6 : class
            where C7 : class
            where C8 : class
        {
            return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>(_archetypes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick()
        {
            _worldInfo.EntityCount = _archetypes.EntityCount;
            _worldInfo.UnusedEntityCount = _archetypes.UnusedIds.Count;
            _worldInfo.AllocatedEntityCount = _archetypes.Meta.Length;
            _worldInfo.ArchetypeCount = _archetypes.Tables.Count;
            // info.RelationCount = relationCount;
            _worldInfo.ElementCount = _archetypes.Tables[_archetypes.Meta[_world.Identity.Id].TableId].Types.Count;
            _worldInfo.CachedQueryCount = _archetypes.Queries.Count;

            _triggerLifeTimeSystem.Run(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Entity GetTypeEntity(Type type)
        {
            return _archetypes.GetTypeEntity(type);
        }
    }

    public sealed class WorldInfo
    {
        public readonly int WorldId;
        public int EntityCount;
        public int UnusedEntityCount;
        public int AllocatedEntityCount;

        public int ArchetypeCount;

        // public int RelationCount;
        public int ElementCount;
        public int CachedQueryCount;

        public WorldInfo(int id)
        {
            WorldId = id;
        }
    }
}