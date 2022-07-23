using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public sealed class World : Object
{
    static int worldCount;

    readonly Entity _world;

    EntityMeta[] _entities = new EntityMeta[512];

    readonly Queue<Identity> _unusedIds = new();

    readonly List<Table> _tables = new();

    readonly Dictionary<int, Query> _queries = new();

    int _entityCount;

    internal readonly List<(Type, TimeSpan)> SystemExecutionTimes = new();

    readonly TriggerLifeTimeSystem _triggerLifeTimeSystem = new();

    readonly List<TableOperation> _tableOperations = new();

    readonly Dictionary<StorageType, List<Table>> _tablesByType = new();
    readonly Dictionary<Identity, List<StorageType>> _typesByRelationTarget = new();
    readonly Dictionary<int, HashSet<StorageType>> _relationsByTypes = new();

    readonly Dictionary<Type, Identity> _typeIdentities = new();

    int _lockCount;
    bool _isLocked;

    public World()
    {
        AddTable(new SortedSet<StorageType> { StorageType.Create<Entity>(Identity.None) });

        _world = Spawn();

        AddElement(new WorldInfo(++worldCount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Spawn()
    {
        var identity = _unusedIds.Count > 0 ? _unusedIds.Dequeue() : new Identity(++_entityCount);

        var table = _tables[0];

        var row = table.Add(identity);

        if (_entities.Length == _entityCount) Array.Resize(ref _entities, _entityCount << 1);

        _entities[identity.Id] = new EntityMeta(identity, table.Id, row);

        var entity = new Entity(identity);

        table.Storages[0].SetValue(entity, row);

        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn(Identity identity)
    {
        if (!IsAlive(identity)) return;

        if (_isLocked)
        {
            _tableOperations.Add(new TableOperation { Despawn = true, Identity = identity });
            return;
        }

        ref var meta = ref _entities[identity.Id];

        var table = _tables[meta.TableId];

        table.Remove(meta.Row);

        meta.Row = 0;
        meta.Identity = Identity.None;

        _unusedIds.Enqueue(identity);

        if (!_typesByRelationTarget.TryGetValue(identity, out var list))
        {
            return;
        }

        Lock();

        foreach (var type in list)
        {
            var tablesWithType = _tablesByType[type];

            foreach (var tableWithType in tablesWithType)
            {
                for (var i = 0; i < tableWithType.Count; i++)
                {
                    RemoveComponent(type, tableWithType.Entities[i]);
                }
            }
        }

        Unlock();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Send<T>(T trigger) where T : class
    {
        if (trigger is null) throw new Exception("trigger cannot be null");

        var entity = Spawn();
        AddComponent(entity.Identity, new SystemList());
        AddComponent(entity.Identity, new LifeTime());
        AddComponent(entity.Identity, new Trigger<T>(trigger));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TriggerQuery<T> Receive<T>(ISystem system) where T : class
    {
        var mask = new Mask();

        mask.Has(StorageType.Create<Trigger<T>>(Identity.None));
        mask.Has(StorageType.Create<SystemList>(Identity.None));
        
        var matchingTables = new List<Table>();

        var type = mask.HasTypes[0];
        if (!_tablesByType.TryGetValue(type, out var typeTables))
        {
            typeTables = new List<Table>();
            _tablesByType[type] = typeTables;
        }

        foreach (var table in typeTables)
        {
            if (!IsMaskCompatibleWith(mask, table)) continue;

            matchingTables.Add(table);
        }
        
        return new TriggerQuery<T>(this, mask, matchingTables, system.GetType());
    }

    public void AddElement<T>(T element) where T : class
    {
        AddComponent(_world.Identity, new Element<T> { Value = element });
    }

    public T GetElement<T>() where T : class
    {
        return GetComponent<Element<T>>(_world.Identity).Value;
    }

    public void ReplaceElement<T>(T element) where T : class
    {
        GetComponent<Element<T>>(_world.Identity).Value = element;
    }

    public bool HasElement<T>() where T : class
    {
        return HasComponent<Element<T>>(_world.Identity);
    }

    public void RemoveElement<T>() where T : class
    {
        RemoveComponent<Element<T>>(_world.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent<T>(Identity identity, T data = default, Identity target = default) where T : class, new()
    {
        var type = StorageType.Create<T>(target);
        if (!type.IsTag && data == null) data = new T();
        AddComponent(type, identity, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void AddComponent(StorageType type, Identity identity, object data = default)
    {
        if (_isLocked)
        {
            _tableOperations.Add(new TableOperation { Add = true, Identity = identity, Type = type, Data = data });
            return;
        }

        ref var meta = ref _entities[identity.Id];
        var oldTable = _tables[meta.TableId];
        var oldEdge = oldTable.GetTableEdge(type);

        var newTable = oldEdge.Add;

        if (newTable == null)
        {
            var newTypes = oldTable.Types.ToList();
            newTypes.Add(type);
            newTable = AddTable(new SortedSet<StorageType>(newTypes));
            oldEdge.Add = newTable;

            var newEdge = newTable.GetTableEdge(type);
            newEdge.Remove = oldTable;
        }

        var newRow = Table.MoveEntry(identity, meta.Row, oldTable, newTable);

        meta.Row = newRow;
        meta.TableId = newTable.Id;

        if (type.IsTag) return;

        var storage = newTable.GetStorage(type);
        storage.SetValue(data, newRow);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponent<T>(Identity identity, Identity target = default) where T : class
    {
        var type = StorageType.Create<T>(target);

        var meta = _entities[identity.Id];
        var table = _tables[meta.TableId];
        var storage = (T[])table.GetStorage(type); // return storages[indices[type]]
        return ref storage[meta.Row];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Identity identity, Identity target = default) where T : class
    {
        var meta = _entities[identity.Id];

        if (meta.Identity == Identity.None) return false;

        var type = StorageType.Create<T>(target);
        return _tables[meta.TableId].Types.Contains(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent<T>(Identity identity, Identity target = default) where T : class
    {
        var type = StorageType.Create<T>(target);
        RemoveComponent(type, identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void RemoveComponent(StorageType type, Identity identity)
    {
        if (_isLocked)
        {
            _tableOperations.Add(new TableOperation { Add = false, Identity = identity, Type = type });
            return;
        }

        ref var meta = ref _entities[identity.Id];
        var oldTable = _tables[meta.TableId];
        var oldEdge = oldTable.GetTableEdge(type);

        var newTable = oldEdge.Remove;

        if (newTable == null)
        {
            var newTypes = oldTable.Types.ToList();
            newTypes.Remove(type);
            newTable = AddTable(new SortedSet<StorageType>(newTypes));
            oldEdge.Remove = newTable;

            var newEdge = newTable.GetTableEdge(type);
            newEdge.Add = oldTable;

            _tables.Add(newTable);
        }

        var newRow = Table.MoveEntry(identity, meta.Row, oldTable, newTable);

        meta.Row = newRow;
        meta.TableId = newTable.Id;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query GetQuery(Mask mask, Func<World, Mask, List<Table>, Query> createQuery)
    {
        var hash = mask.GetHashCode();

        if (_queries.TryGetValue(hash, out var query)) return query;

        var matchingTables = new List<Table>();

        var type = mask.HasTypes[0];
        if (!_tablesByType.TryGetValue(type, out var typeTables))
        {
            typeTables = new List<Table>();
            _tablesByType[type] = typeTables;
        }

        foreach (var table in typeTables)
        {
            if (!IsMaskCompatibleWith(mask, table)) continue;

            matchingTables.Add(table);
        }

        query = createQuery(this, mask, matchingTables);
        _queries.Add(hash, query);

        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IsMaskCompatibleWith(Mask mask, Table table)
    {
        var has = ListPool<StorageType>.Get();
        var not = ListPool<StorageType>.Get();
        var any = ListPool<StorageType>.Get();

        var hasAnyTarget = ListPool<StorageType>.Get();
        var notAnyTarget = ListPool<StorageType>.Get();
        var anyAnyTarget = ListPool<StorageType>.Get();

        foreach (var type in mask.HasTypes)
        {
            if (type.Identity == Identity.Any) hasAnyTarget.Add(type);
            else has.Add(type);
        }

        foreach (var type in mask.NotTypes)
        {
            if (type.Identity == Identity.Any) notAnyTarget.Add(type);
            else not.Add(type);
        }

        foreach (var type in mask.AnyTypes)
        {
            if (type.Identity == Identity.Any) anyAnyTarget.Add(type);
            else any.Add(type);
        }

        var matchesComponents = table.Types.IsSupersetOf(has);
        matchesComponents &= !table.Types.Overlaps(not);
        matchesComponents &= mask.AnyTypes.Count == 0 || table.Types.Overlaps(any);

        var matchesRelation = true;

        foreach (var type in hasAnyTarget)
        {
            if (!_relationsByTypes.TryGetValue(type.TypeId, out var list))
            {
                matchesRelation = false;
                continue;
            }

            matchesRelation &= table.Types.Overlaps(list);
        }

        ListPool<StorageType>.Add(has);
        ListPool<StorageType>.Add(not);
        ListPool<StorageType>.Add(any);
        ListPool<StorageType>.Add(hasAnyTarget);
        ListPool<StorageType>.Add(notAnyTarget);
        ListPool<StorageType>.Add(anyAnyTarget);

        return matchesComponents && matchesRelation;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsAlive(Identity identity)
    {
        return _entities[identity.Id].Identity != Identity.None;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref EntityMeta GetEntityMeta(Identity identity)
    {
        return ref _entities[identity.Id];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Table GetTable(int tableId)
    {
        return _tables[tableId];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Entity[] GetTargets<T>(Identity identity) where T : class
    {
        var type = StorageType.Create<T>(Identity.None);

        var meta = _entities[identity.Id];
        var table = _tables[meta.TableId];

        var list = ListPool<Entity>.Get();

        foreach (var storageType in table.Types)
        {
            if (!storageType.IsRelation || storageType.TypeId != type.TypeId) continue;
            list.Add(new Entity(storageType.Identity));
        }

        var targetEntities = list.ToArray();
        ListPool<Entity>.Add(list);

        return targetEntities;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Identity GetTypeIdentity(Type type)
    {
        if (_typeIdentities.TryGetValue(type, out var identity)) return identity;

        var entity = Spawn();
        _typeIdentities.Add(type, entity.Identity);
        return identity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Table AddTable(SortedSet<StorageType> types)
    {
        var table = new Table(_tables.Count, this, types);
        _tables.Add(table);

        foreach (var type in types)
        {
            if (!_tablesByType.TryGetValue(type, out var tableList))
            {
                tableList = new List<Table>();
                _tablesByType[type] = tableList;
            }

            tableList.Add(table);

            if (!type.IsRelation) continue;

            if (!_typesByRelationTarget.TryGetValue(type.Identity, out var typeList))
            {
                typeList = new List<StorageType>();
                _typesByRelationTarget[type.Identity] = typeList;
            }

            typeList.Add(type);

            if (!_relationsByTypes.TryGetValue(type.TypeId, out var relationTypeSet))
            {
                relationTypeSet = new HashSet<StorageType>();
                _relationsByTypes[type.TypeId] = relationTypeSet;
            }

            relationTypeSet.Add(type);
        }

        foreach (var query in _queries.Values.Where(query => IsMaskCompatibleWith(query.Mask, table)))
        {
            query.AddTable(table);
        }

        return table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ApplyTableOperations()
    {
        foreach (var op in _tableOperations)
        {
            if (!IsAlive(op.Identity)) continue;

            if (op.Despawn) Despawn(op.Identity);
            else if (op.Add) AddComponent(op.Type, op.Identity, op.Data);
            else RemoveComponent(op.Type, op.Identity);
        }

        _tableOperations.Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Lock()
    {
        _lockCount++;
        _isLocked = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Unlock()
    {
        _lockCount--;
        if (_lockCount != 0) return;
        _isLocked = false;

        ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Tick()
    {
        var info = GetElement<WorldInfo>();

        info.EntityCount = _entityCount;
        info.UnusedEntityCount = _unusedIds.Count;
        info.AllocatedEntityCount = _entities.Length;
        info.ArchetypeCount = _tables.Count;
        // info.RelationCount = relationCount;
        info.ElementCount = _tables[_entities[_world.Identity.Id].TableId].Types.Count;
        info.SystemCount = SystemExecutionTimes.Count;
        info.CachedQueryCount = _queries.Count;

        info.SystemExecutionTimes.Clear();
        info.SystemExecutionTimes.AddRange(SystemExecutionTimes);

        _triggerLifeTimeSystem.Run(this);

        SystemExecutionTimes.Clear();
    }

    struct TableOperation
    {
        public bool Despawn;
        public bool Add;
        public StorageType Type;
        public Identity Identity;
        public object Data;
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
    public int SystemCount;
    public List<(Type, TimeSpan)> SystemExecutionTimes;
    public int CachedQueryCount;

    public WorldInfo(int id)
    {
        WorldId = id;
        SystemExecutionTimes = new List<(Type, TimeSpan)>();
    }
}