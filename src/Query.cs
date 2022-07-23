using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class Query : Object
{
    public readonly List<Table> Tables;

    internal readonly World World;
    internal readonly Mask Mask;

    protected readonly List<Array[]> Storages = new();
    protected readonly Dictionary<int, int> Indices = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query(World world, Mask mask, List<Table> tables)
    {
        Tables = tables;
        World = world;
        Mask = mask;

        UpdateStorages();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        return Indices.ContainsKey(meta.TableId);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AddTable(Table table)
    {
        Tables.Add(table);
        UpdateStorages();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual Array[] GetStorages(Table table)
    {
        throw new Exception("Invalid Enumerator");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorages()
    {
        Storages.Clear();
        Indices.Clear();

        for (var i = 0; i < Tables.Count; i++)
        {
            Indices.Add(Tables[i].Id, i);
            Storages.Add(GetStorages(Tables[i]));
        }
    }
}

public class TriggerQuery<C> : Query
    where C : class
{
    readonly Type _systemType;

    public TriggerQuery(World world, Mask mask, List<Table> tables, Type systemType) : base(world, mask, tables)
    {
        _systemType = systemType;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[] { table.GetStorage<Trigger<C>>(Identity.None) };
    }

    public TriggerEnumerator<C> GetEnumerator()
    {
        return new TriggerEnumerator<C>(World, Tables, _systemType);
    }
}

public class Query<C> : Query
    where C : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[] { table.GetStorage<C>(Identity.None) };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public C Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storage = (C[])Storages[Indices[meta.TableId]][0];
        return storage[meta.Row];
    }

    public Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(World, Tables);
    }
}

public class Query<C1, C2> : Query
    where C1 : class
    where C2 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        return (storage1[meta.Row], storage2[meta.Row]);
    }

    public Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(World, Tables);
    }
}

public class Query<C1, C2, C3> : Query
    where C1 : class
    where C2 : class
    where C3 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row]);
    }

    public Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4> : Query
    where C1 : class
    where C2 : class
    where C3 : class
    where C4 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
            table.GetStorage<C4>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3, C4) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        var storage4 = (C4[])storages[3];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row]);
    }

    public Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5> : Query
    where C1 : class
    where C2 : class
    where C3 : class
    where C4 : class
    where C5 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
            table.GetStorage<C4>(Identity.None),
            table.GetStorage<C5>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3, C4, C5) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        var storage4 = (C4[])storages[3];
        var storage5 = (C5[])storages[4];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row]);
    }

    public Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6> : Query
    where C1 : class
    where C2 : class
    where C3 : class
    where C4 : class
    where C5 : class
    where C6 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
            table.GetStorage<C4>(Identity.None),
            table.GetStorage<C5>(Identity.None),
            table.GetStorage<C6>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3, C4, C5, C6) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        var storage4 = (C4[])storages[3];
        var storage5 = (C5[])storages[4];
        var storage6 = (C6[])storages[5];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row],
            storage6[meta.Row]);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6, C7> : Query
    where C1 : class
    where C2 : class
    where C3 : class
    where C4 : class
    where C5 : class
    where C6 : class
    where C7 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
            table.GetStorage<C4>(Identity.None),
            table.GetStorage<C5>(Identity.None),
            table.GetStorage<C6>(Identity.None),
            table.GetStorage<C7>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3, C4, C5, C6, C7) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        var storage4 = (C4[])storages[3];
        var storage5 = (C5[])storages[4];
        var storage6 = (C6[])storages[5];
        var storage7 = (C7[])storages[6];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row],
            storage6[meta.Row], storage7[meta.Row]);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6, C7, C8> : Query
    where C1 : class
    where C2 : class
    where C3 : class
    where C4 : class
    where C5 : class
    where C6 : class
    where C7 : class
    where C8 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
            table.GetStorage<C4>(Identity.None),
            table.GetStorage<C5>(Identity.None),
            table.GetStorage<C6>(Identity.None),
            table.GetStorage<C7>(Identity.None),
            table.GetStorage<C8>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3, C4, C5, C6, C7, C8) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        var storage4 = (C4[])storages[3];
        var storage5 = (C5[])storages[4];
        var storage6 = (C6[])storages[5];
        var storage7 = (C7[])storages[6];
        var storage8 = (C8[])storages[7];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row],
            storage6[meta.Row], storage7[meta.Row], storage8[meta.Row]);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6, C7, C8, C9> : Query
    where C1 : class
    where C2 : class
    where C3 : class
    where C4 : class
    where C5 : class
    where C6 : class
    where C7 : class
    where C8 : class
    where C9 : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Array[] GetStorages(Table table)
    {
        return new Array[]
        {
            table.GetStorage<C1>(Identity.None),
            table.GetStorage<C2>(Identity.None),
            table.GetStorage<C3>(Identity.None),
            table.GetStorage<C4>(Identity.None),
            table.GetStorage<C5>(Identity.None),
            table.GetStorage<C6>(Identity.None),
            table.GetStorage<C7>(Identity.None),
            table.GetStorage<C8>(Identity.None),
            table.GetStorage<C9>(Identity.None),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (C1, C2, C3, C4, C5, C6, C7, C8, C9) Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        var storages = Storages[Indices[meta.TableId]];
        var storage1 = (C1[])storages[0];
        var storage2 = (C2[])storages[1];
        var storage3 = (C3[])storages[2];
        var storage4 = (C4[])storages[3];
        var storage5 = (C5[])storages[4];
        var storage6 = (C6[])storages[5];
        var storage7 = (C7[])storages[6];
        var storage8 = (C8[])storages[7];
        var storage9 = (C9[])storages[8];
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row],
            storage6[meta.Row], storage7[meta.Row], storage8[meta.Row], storage9[meta.Row]);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9>(World, Tables);
    }
}

public class Enumerator : IEnumerator, IDisposable
{
    protected readonly List<Table> Tables;

    protected int TableIndex;
    protected int EntityIndex;

    readonly World _world;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected Enumerator(World world, List<Table> tables)
    {
        _world = world;
        Tables = tables;

        world.Lock();

        Reset();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (TableIndex == Tables.Count) return false;

        if (++EntityIndex < Tables[TableIndex].Count) return true;

        EntityIndex = 0;
        TableIndex++;

        while (TableIndex < Tables.Count && Tables[TableIndex].Count == 0)
        {
            TableIndex++;
        }

        UpdateStorage();

        return TableIndex < Tables.Count && EntityIndex < Tables[TableIndex].Count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        TableIndex = 0;
        EntityIndex = -1;

        UpdateStorage();
    }

    object IEnumerator.Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => throw new Exception("Invalid Enumerator");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        _world.Unlock();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void UpdateStorage()
    {
        throw new Exception("Invalid Enumerator");
    }
}

public class TriggerEnumerator<C> : Enumerator
{
    Trigger<C>[] _storage;
    SystemList[] _systemLists;
    readonly Type _systemType;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TriggerEnumerator(World world, List<Table> tables, Type systemType) : base(world, tables)
    {
        _systemType = systemType;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new bool MoveNext()
    {
        if (TableIndex == Tables.Count) return false;

        EntityIndex++;

        while (Tables[TableIndex].Count > EntityIndex && _systemLists[EntityIndex].List.Contains(_systemType))
        {
            EntityIndex++;
        }

        if (EntityIndex < Tables[TableIndex].Count) return true;

        EntityIndex = 0;
        TableIndex++;

        while (TableIndex < Tables.Count && Tables[TableIndex].Count == 0)
        {
            TableIndex++;
        }

        UpdateStorage();

        return TableIndex < Tables.Count && EntityIndex < Tables[TableIndex].Count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage = Tables[TableIndex].GetStorage<Trigger<C>>(Identity.None);
        _systemLists = Tables[TableIndex].GetStorage<SystemList>(Identity.None);
    }

    public C Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            _systemLists[EntityIndex].List.Add(_systemType);
            return _storage[EntityIndex].Value;
        }
    }
}

public class Enumerator<C> : Enumerator
{
    C[] _storage;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage = Tables[TableIndex].GetStorage<C>(Identity.None);
    }

    public C Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _storage[EntityIndex];
    }
}

public class Enumerator<C1, C2> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
    }

    public (C1, C2) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
    }

    public (C1, C2, C3) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;
    C4[] _storage4;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        _storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
    }

    public (C1, C2, C3, C4) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex], _storage4[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;
    C4[] _storage4;
    C5[] _storage5;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        _storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        _storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
    }

    public (C1, C2, C3, C4, C5) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex], _storage4[EntityIndex],
            _storage5[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;
    C4[] _storage4;
    C5[] _storage5;
    C6[] _storage6;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        _storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        _storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        _storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
    }

    public (C1, C2, C3, C4, C5, C6) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex], _storage4[EntityIndex],
            _storage5[EntityIndex], _storage6[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6, C7> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;
    C4[] _storage4;
    C5[] _storage5;
    C6[] _storage6;
    C7[] _storage7;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        _storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        _storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        _storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
        _storage7 = Tables[TableIndex].GetStorage<C7>(Identity.None);
    }

    public (C1, C2, C3, C4, C5, C6, C7) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex], _storage4[EntityIndex],
            _storage5[EntityIndex], _storage6[EntityIndex], _storage7[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;
    C4[] _storage4;
    C5[] _storage5;
    C6[] _storage6;
    C7[] _storage7;
    C8[] _storage8;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        _storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        _storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        _storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
        _storage7 = Tables[TableIndex].GetStorage<C7>(Identity.None);
        _storage8 = Tables[TableIndex].GetStorage<C8>(Identity.None);
    }

    public (C1, C2, C3, C4, C5, C6, C7, C8) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex], _storage4[EntityIndex],
            _storage5[EntityIndex], _storage6[EntityIndex], _storage7[EntityIndex], _storage8[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9> : Enumerator
{
    C1[] _storage1;
    C2[] _storage2;
    C3[] _storage3;
    C4[] _storage4;
    C5[] _storage5;
    C6[] _storage6;
    C7[] _storage7;
    C8[] _storage8;
    C9[] _storage9;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        _storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        _storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        _storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        _storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        _storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        _storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
        _storage7 = Tables[TableIndex].GetStorage<C7>(Identity.None);
        _storage8 = Tables[TableIndex].GetStorage<C8>(Identity.None);
        _storage9 = Tables[TableIndex].GetStorage<C9>(Identity.None);
    }

    public (C1, C2, C3, C4, C5, C6, C7, C8, C9) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_storage1[EntityIndex], _storage2[EntityIndex], _storage3[EntityIndex], _storage4[EntityIndex],
            _storage5[EntityIndex], _storage6[EntityIndex], _storage7[EntityIndex], _storage8[EntityIndex],
            _storage9[EntityIndex]);
    }
}