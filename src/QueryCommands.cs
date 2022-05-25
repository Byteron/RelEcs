using System;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class QueryCommands
{
    internal readonly World World;
    protected readonly Mask Mask;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands(World world)
    {
        World = world;
        Mask = new Mask();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Has<T>(Entity target = default)
    {
        var typeIndex = StorageType.Create<T>(target?.Identity ?? Identity.None);
        Mask.Has(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Has<T>(Type type)
    {
        var identity = World.GetTypeIdentity(type);
        var typeIndex = StorageType.Create<T>(identity);
        Mask.Has(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Not<T>(Entity target = default)
    {
        var typeIndex = StorageType.Create<T>(target?.Identity ?? Identity.None);
        Mask.Not(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Not<T>(Type type)
    {
        var identity = World.GetTypeIdentity(type);
        var typeIndex = StorageType.Create<T>(identity);
        Mask.Not(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Any<T>(Entity target = default)
    {
        var typeIndex = StorageType.Create<T>(target?.Identity ?? Identity.None);
        Mask.Any(typeIndex);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Any<T>(Type type)
    {
        var identity = World.GetTypeIdentity(type);
        var typeIndex = StorageType.Create<T>(identity);
        Mask.Any(typeIndex);
        return this;
    }
}

public sealed class QueryCommands<C> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C> Has<T>(Entity target = default)
    {
        return (QueryCommands<C>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C> Has<T>(Type type)
    {
        return (QueryCommands<C>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C> Not<T>(Entity target = default)
    {
        return (QueryCommands<C>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C> Not<T>(Type type)
    {
        return (QueryCommands<C>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C> Any<T>(Entity target = default)
    {
        return (QueryCommands<C>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C> Any<T>(Type type)
    {
        return (QueryCommands<C>)base.Any<T>(type);
    }

    public Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(World, World.GetQuery(Mask).Tables);
    }
}

public sealed class QueryCommands<C1, C2> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2>)base.Any<T>(type);
    }
    
    public Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(World, World.GetQuery(Mask).Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3>)base.Any<T>(type);
    }
    
    public Enumerator<C1, C2, C3> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3>(World, query.Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3, C4> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4>)base.Any<T>(type);
    }
    
    public Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3, C4>(World, query.Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3, C4, C5> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5>)base.Any<T>(type);
    }
    
    public Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3, C4, C5>(World, query.Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3, C4, C5, C6> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6>)base.Any<T>(type);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3, C4, C5, C6>(World, query.Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3, C4, C5, C6, C7> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7>)base.Any<T>(type);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(World, query.Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>)base.Any<T>(type);
    }
    
    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(World, query.Tables);
    }
}

public sealed class QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> : QueryCommands
{
    public QueryCommands(World world) : base(world)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>().Has<C9>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Has<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Has<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Has<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Has<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Not<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Not<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Not<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Not<T>(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Any<T>(Entity target = default)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Any<T>(target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Any<T>(Type type)
    {
        return (QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Any<T>(type);
    }
    
    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9> GetEnumerator()
    {
        var query = World.GetQuery(Mask);
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9>(World, query.Tables);
    }
}