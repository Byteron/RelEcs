using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public struct QueryCommands
{
    internal readonly World World;

    readonly Mask mask;
    
    Query query;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands(World world)
    {
        World = world;
        mask = new Mask();
        query = null;
    }
    
    public List<Table> Tables
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            query ??= World.GetQuery(mask);
            return query.Tables;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Has<T>(Entity target = default)
    {
        var typeIndex = StorageType.Create<T>(target.Identity);
        mask.Has(typeIndex);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Has<T>(Type type)
    {
        var identity = World.GetTypeIdentity(type);
        var typeIndex = StorageType.Create<T>(identity);
        mask.Has(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Not<T>(Entity target = default)
    {
        var typeIndex = StorageType.Create<T>(target.Identity);
        mask.Not(typeIndex);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Not<T>(Type type)
    {
        var identity = World.GetTypeIdentity(type);
        var typeIndex = StorageType.Create<T>(identity);
        mask.Not(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Any<T>(Entity target = default)
    {
        var typeIndex = StorageType.Create<T>(target.Identity);
        mask.Any(typeIndex);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Any<T>(Type type)
    {
        var identity = World.GetTypeIdentity(type);
        var typeIndex = StorageType.Create<T>(identity);
        mask.Any(typeIndex);
        return this;
    }
}