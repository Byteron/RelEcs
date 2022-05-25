using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class Query
{
    public readonly List<Table> Tables;
    
    internal readonly Mask Mask;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query(Mask mask, List<Table> tables)
    {
        Mask = mask;
        Tables = tables;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddTable(Table table)
    {
        Tables.Add(table);
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
        
        foreach (var table in tables)
        {
            table.Lock();
        }
        
        Reset();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (TableIndex == Tables.Count) return false;

        if (++EntityIndex < Tables[TableIndex].Count) return true;
        
        EntityIndex = 0;
        TableIndex++;

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
        foreach (var table in Tables)
        {
            table.Unlock();
        }
        
        world.ApplyTableOperations();
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