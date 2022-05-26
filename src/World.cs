using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public sealed class World
{
    static int worldCount;

    readonly Entity world;

    EntityMeta[] entities = new EntityMeta[512];

    readonly Queue<Identity> unusedIds = new();

    readonly List<Table> tables = new();

    readonly Dictionary<int, Query> queries = new();

    int entityCount;

    internal readonly List<(Type, TimeSpan)> SystemExecutionTimes = new();

    readonly TriggerLifeTimeSystem triggerLifeTimeSystem = new();
    
    readonly List<TableOperation> tableOperations = new();
    
    readonly Dictionary<StorageType, List<Table>> tablesByType = new();
    readonly Dictionary<Identity, List<StorageType>> typesByRelationTarget = new();
    readonly Dictionary<int, HashSet<StorageType>> relationsByTypes = new();

    readonly Dictionary<Type, Identity> typeIdentities = new();
    
    public World()
    {
        AddTable(new SortedSet<StorageType> { StorageType.Create<Entity>(Identity.None)});

        world = Spawn();

        AddElement(new WorldInfo(++worldCount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Spawn()
    {
        var identity = unusedIds.Count > 0 ? unusedIds.Dequeue() : new Identity(++entityCount);

        var table = tables[0];

        var row = table.Add(identity);

        if (entities.Length == entityCount) Array.Resize(ref entities, entityCount << 1);

        entities[identity.Id] = new EntityMeta(identity, table.Id, row);

        var entity = new Entity(this, identity);

        table.Storages[0].SetValue(entity, row);
        
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn(Identity identity)
    {
        if (!IsAlive(identity)) return;

        unusedIds.Enqueue(identity);

        ref var meta = ref entities[identity.Id];

        var table = tables[meta.TableId];
        table.Remove(meta.Row);

        meta.Row = 0;
        meta.Identity = Identity.None;

        if (!typesByRelationTarget.TryGetValue(identity, out var list))
        {
            return;
        }

        foreach (var type in list)
        {
            var tablesWithType = tablesByType[type];

            foreach (var tableWithType in tablesWithType)
            {
                tableWithType.Lock();
                for (var i = 0; i < tableWithType.Count; i++)
                {
                    RemoveComponent(type, tableWithType.Entities[i]);
                }

                tableWithType.Unlock();
            }

            ApplyTableOperations();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Send<T>(T trigger) where T : class
    {
        if (trigger is null) throw new Exception("trigger cannot be null");
        
        var entity = Spawn();
        AddComponent(entity.Identity, new TriggerSystemList(ListPool<Type>.Get()));
        AddComponent(entity.Identity, new TriggerLifeTime());
        AddComponent(entity.Identity, new Trigger<T>(trigger));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Receive<T>(ISystem system, Action<T> action) where T : class
    {
        var systemType = system.GetType();

        var mask = new Mask();

        mask.Has(StorageType.Create<Trigger<T>>(Identity.None));
        mask.Has(StorageType.Create<TriggerSystemList>(Identity.None));

        var query = GetQuery(mask);

        foreach (var table in query.Tables)
        {
            var triggerStorage = table.GetStorage<Trigger<T>>(Identity.None);
            var systemStorage = table.GetStorage<TriggerSystemList>(Identity.None);

            for (var i = 0; i < table.Count; i++)
            {
                var systemList = systemStorage[i];

                if (systemList.List.Contains(systemType)) continue;

                systemList.List.Add(systemType);
                action(triggerStorage[i].Value);
            }
        }
    }

    public void AddElement<T>(T element) where T : class
    {
        world.Add(new Element<T> { Value = element });
    }

    public T GetElement<T>() where T : class
    {
        return world.Get<Element<T>>().Value;
    }

    public void ReplaceElement<T>(T element) where T : class
    {
        world.Get<Element<T>>().Value = element;
    }

    public bool HasElement<T>() where T : class
    {
        return world.Has<Element<T>>();
    }

    public void RemoveElement<T>() where T : class
    {
        world.Remove<Element<T>>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent<T>(Identity identity, T data = default, Identity target = default) where T: class, new()
    {
        var type = StorageType.Create<T>(target);
        if (!type.IsTag && data == null) data = new T();
        AddComponent(type, identity, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void AddComponent(StorageType type, Identity identity, object data = default)
    {
        ref var meta = ref entities[identity.Id];
        var oldTable = tables[meta.TableId];
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

        if (oldTable.IsLocked || newTable.IsLocked)
        {
            tableOperations.Add(new TableOperation { Add = true, Identity = identity, Type = type, Data = data });
            return;
        }

        var newRow = Table.MoveEntry(identity, meta.Row, oldTable, newTable);

        meta.Row = newRow;
        meta.TableId = newTable.Id;
        
        if (type.IsTag) return;
        
        var storage = newTable.GetStorage(type);
        storage.SetValue(data, newRow);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponent<T>(Identity identity, Identity target = default) where T: class
    {
        var type = StorageType.Create<T>(target);

        var meta = entities[identity.Id];
        var table = tables[meta.TableId];
        var storage = (T[])table.GetStorage(type); // return storages[indices[type]]
        return ref storage[meta.Row];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Identity identity, Identity target = default) where T: class
    {
        var meta = entities[identity.Id];

        if (meta.Identity == Identity.None) return false;

        var type = StorageType.Create<T>(target);
        return tables[meta.TableId].Types.Contains(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent<T>(Identity identity, Identity target = default) where T: class
    {
        var type = StorageType.Create<T>(target);
        RemoveComponent(type, identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void RemoveComponent(StorageType type, Identity identity)
    {
        ref var meta = ref entities[identity.Id];
        var oldTable = tables[meta.TableId];
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

            tables.Add(newTable);
        }

        if (oldTable.IsLocked || newTable.IsLocked)
        {
            tableOperations.Add(new TableOperation { Add = false, Identity = identity, Type = type });
            return;
        }

        var newRow = Table.MoveEntry(identity, meta.Row, oldTable, newTable);

        meta.Row = newRow;
        meta.TableId = newTable.Id;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query GetQuery(Mask mask)
    {
        var hash = mask.GetHashCode();

        if (queries.TryGetValue(hash, out var query)) return query;
        
        var matchingTables = new List<Table>();

        var type = mask.HasTypes[0];
        if (!tablesByType.TryGetValue(type, out var typeTables))
        {
            typeTables = new List<Table>();
            tablesByType[type] = typeTables;
        }

        foreach (var table in typeTables)
        {
            if (!IsMaskCompatibleWith(mask, table)) continue;

            matchingTables.Add(table);
        }

        query = new Query(mask, matchingTables);
        queries.Add(hash, query);

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

        foreach (var type in mask.HasTypes) if (type.Identity == Identity.Any) hasAnyTarget.Add(type); else has.Add(type);
        foreach (var type in mask.NotTypes) if (type.Identity == Identity.Any) notAnyTarget.Add(type); else not.Add(type);
        foreach (var type in mask.AnyTypes) if (type.Identity == Identity.Any) anyAnyTarget.Add(type); else any.Add(type);
        
        var matchesComponents = table.Types.IsSupersetOf(has);
        matchesComponents &= !table.Types.Overlaps(not);
        matchesComponents &= mask.AnyTypes.Count == 0 || table.Types.Overlaps(any);

        var matchesRelation = true;
        
        foreach (var type in hasAnyTarget)
        {
            if (!relationsByTypes.TryGetValue(type.TypeId, out var list))
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
        return entities[identity.Id].Identity != Identity.None;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref EntityMeta GetEntityMeta(Identity identity)
    {
        return ref entities[identity.Id];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Entity[] GetTargets<T>(Identity identity) where T : class
    {
        var type = StorageType.Create<T>(Identity.None);
        
        var meta = entities[identity.Id];
        var table = tables[meta.TableId];
        
        var list = ListPool<Entity>.Get();

        foreach (var storageType in table.Types)
        {
            if (!storageType.IsRelation || storageType.TypeId != type.TypeId) continue;
            list.Add(new Entity(this, storageType.Identity));
        }
        
        var targetEntities = list.ToArray();
        ListPool<Entity>.Add(list);

        return targetEntities;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Identity GetTypeIdentity(Type type)
    {
        if (typeIdentities.TryGetValue(type, out var identity)) return identity;

        var entity = Spawn();
        typeIdentities.Add(type, entity.Identity);
        return identity;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Table AddTable(SortedSet<StorageType> types)
    {
        var table = new Table(tables.Count, this, types);
        tables.Add(table);

        foreach (var type in types)
        {
            if (!tablesByType.TryGetValue(type, out var tableList))
            {
                tableList = new List<Table>();
                tablesByType[type] = tableList;
            }

            tableList.Add(table);

            if (!type.IsRelation) continue;

            if (!typesByRelationTarget.TryGetValue(type.Identity, out var typeList))
            {
                typeList = new List<StorageType>();
                typesByRelationTarget[type.Identity] = typeList;
            }

            typeList.Add(type);

            if (!relationsByTypes.TryGetValue(type.TypeId, out var relationTypeSet))
            {
                relationTypeSet = new HashSet<StorageType>();
                relationsByTypes[type.TypeId] = relationTypeSet;
            }

            relationTypeSet.Add(type);
        }

        foreach (var query in queries.Values.Where(query => IsMaskCompatibleWith(query.Mask, table)))
        {
            query.AddTable(table);
        }

        return table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyTableOperations()
    {
        for (var i = tableOperations.Count - 1; i >= 0; i--)
        {
            var op = tableOperations[i];

            if (op.Add)
            {
                AddComponent(op.Type, op.Identity, op.Data);
            }
            else
            {
                RemoveComponent(op.Type, op.Identity);
            }

            tableOperations.RemoveAt(i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Tick()
    {
        var info = GetElement<WorldInfo>();

        info.EntityCount = entityCount;
        info.UnusedEntityCount = unusedIds.Count;
        info.AllocatedEntityCount = entities.Length;
        info.ArchetypeCount = tables.Count;
        // info.RelationCount = relationCount;
        info.ElementCount = tables[entities[world.Identity.Id].TableId].Types.Count;
        info.SystemCount = SystemExecutionTimes.Count;
        info.CachedQueryCount = queries.Count;

        info.SystemExecutionTimes.Clear();
        info.SystemExecutionTimes.AddRange(SystemExecutionTimes);

        triggerLifeTimeSystem.Run(this);

        SystemExecutionTimes.Clear();
    }

    struct TableOperation
    {
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