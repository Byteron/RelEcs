using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class Query
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
    public void AddTable(Table table)
    {
        Tables.Add(table);
        UpdateStorages();
    }
    
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual Array[] GetStorages(Table table)
    {
        throw new Exception("Invalid Enumerator");
    }
}

public class Query<C> : Query
    where C : class
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Identity);
        return Indices.ContainsKey(meta.TableId);
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row], storage6[meta.Row]);
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row], storage6[meta.Row], storage7[meta.Row]);
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row], storage6[meta.Row], storage7[meta.Row], storage8[meta.Row]);
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
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables) { }
    
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
        return (storage1[meta.Row], storage2[meta.Row], storage3[meta.Row], storage4[meta.Row], storage5[meta.Row], storage6[meta.Row], storage7[meta.Row], storage8[meta.Row], storage9[meta.Row]);
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
    
    World world;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected Enumerator(World world, List<Table> tables)
    {
        this.world = world;
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
        world.Unlock();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void UpdateStorage()
    {
        throw new Exception("Invalid Enumerator");
    }
}

public class Enumerator<C> : Enumerator
{
    C[] storage;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage = Tables[TableIndex].GetStorage<C>(Identity.None);
    }
    
    public C Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => storage[EntityIndex];
    }
}

public class Enumerator<C1, C2> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
    }
    
    public (C1, C2) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
    }
    
    public (C1, C2, C3) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
    }
    
    public (C1, C2, C3, C4) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex], storage4[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
    }
    
    public (C1, C2, C3, C4, C5) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex], storage4[EntityIndex], storage5[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
    }
    
    public (C1, C2, C3, C4, C5, C6) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex], storage4[EntityIndex], storage5[EntityIndex], storage6[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6, C7> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    C7[] storage7;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
        storage7 = Tables[TableIndex].GetStorage<C7>(Identity.None);
    }
    
    public (C1, C2, C3, C4, C5, C6, C7) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex], storage4[EntityIndex], storage5[EntityIndex], storage6[EntityIndex], storage7[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    C7[] storage7;
    C8[] storage8;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
        storage7 = Tables[TableIndex].GetStorage<C7>(Identity.None);
        storage8 = Tables[TableIndex].GetStorage<C8>(Identity.None);
    }
    
    public (C1, C2, C3, C4, C5, C6, C7, C8) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex], storage4[EntityIndex], storage5[EntityIndex], storage6[EntityIndex], storage7[EntityIndex], storage8[EntityIndex]);
    }
}

public class Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9> : Enumerator
{
    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    C7[] storage7;
    C8[] storage8;
    C9[] storage9;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator(World world, List<Table> tables) : base(world, tables) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void UpdateStorage()
    {
        if (TableIndex == Tables.Count) return;
        storage1 = Tables[TableIndex].GetStorage<C1>(Identity.None);
        storage2 = Tables[TableIndex].GetStorage<C2>(Identity.None);
        storage3 = Tables[TableIndex].GetStorage<C3>(Identity.None);
        storage4 = Tables[TableIndex].GetStorage<C4>(Identity.None);
        storage5 = Tables[TableIndex].GetStorage<C5>(Identity.None);
        storage6 = Tables[TableIndex].GetStorage<C6>(Identity.None);
        storage7 = Tables[TableIndex].GetStorage<C7>(Identity.None);
        storage8 = Tables[TableIndex].GetStorage<C8>(Identity.None);
        storage9 = Tables[TableIndex].GetStorage<C9>(Identity.None);
    }
    
    public (C1, C2, C3, C4, C5, C6, C7, C8, C9) Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (storage1[EntityIndex], storage2[EntityIndex], storage3[EntityIndex], storage4[EntityIndex], storage5[EntityIndex], storage6[EntityIndex], storage7[EntityIndex], storage8[EntityIndex], storage9[EntityIndex]);
    }
}