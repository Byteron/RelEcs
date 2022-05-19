using System;
using System.Runtime.CompilerServices;

namespace RelEcs;

public static class QueryExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2, C3>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>().Has<C3>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2, C3, C4>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>().Has<C3>().Has<C4>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2, C3, C4, C5>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2, C3, C4, C5, C6>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Has<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query)
    {
        query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2, C3>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>().Not<C3>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2, C3, C4>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>().Not<C3>().Not<C4>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2, C3, C4, C5>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2, C3, C4, C5, C6>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>().Not<C6>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>().Not<C6>().Not<C7>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Not<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query)
    {
        query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>().Not<C6>().Not<C7>().Not<C8>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2, C3>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>().Any<C3>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2, C3, C4>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>().Any<C3>().Any<C4>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2, C3, C4, C5>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2, C3, C4, C5, C6>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>().Any<C6>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>().Any<C6>().Any<C7>();
        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands Any<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query)
    {
        query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>().Any<C6>().Any<C7>().Any<C8>();
        return query;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C>(this QueryCommands query, Action<C> action)
    {
        foreach (var table in query.Tables)
        {
            var storage = table.GetStorage<C>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2>(this QueryCommands query, Action<C1, C2> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3>(this QueryCommands query, Action<C1, C2, C3> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i], storage3[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4>(this QueryCommands query, Action<C1, C2, C3, C4> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i], storage3[i], storage4[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5>(this QueryCommands query, Action<C1, C2, C3, C4, C5> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6>(this QueryCommands query,
        Action<C1, C2, C3, C4, C5, C6> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);
            var storage6 = table.GetStorage<C6>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i],
                    storage6[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query,
        Action<C1, C2, C3, C4, C5, C6, C7> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);
            var storage6 = table.GetStorage<C6>(Identity.None);
            var storage7 = table.GetStorage<C7>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i],
                    storage6[i], storage7[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query,
        Action<C1, C2, C3, C4, C5, C6, C7, C8> action)
    {
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);
            var storage6 = table.GetStorage<C6>(Identity.None);
            var storage7 = table.GetStorage<C7>(Identity.None);
            var storage8 = table.GetStorage<C8>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i],
                    storage6[i], storage7[i], storage8[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach(this QueryCommands query, Action<Entity> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]));
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C>(this QueryCommands query, Action<Entity, C> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage = table.GetStorage<C>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2>(this QueryCommands query, Action<Entity, C1, C2> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3>(this QueryCommands query, Action<Entity, C1, C2, C3> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i], storage3[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4>(this QueryCommands query, Action<Entity, C1, C2, C3, C4> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i], storage3[i],
                    storage4[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5>(this QueryCommands query,
        Action<Entity, C1, C2, C3, C4, C5> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i], storage3[i],
                    storage4[i], storage5[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6>(this QueryCommands query,
        Action<Entity, C1, C2, C3, C4, C5, C6> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);
            var storage6 = table.GetStorage<C6>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i], storage3[i],
                    storage4[i], storage5[i], storage6[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query,
        Action<Entity, C1, C2, C3, C4, C5, C6, C7> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);
            var storage6 = table.GetStorage<C6>(Identity.None);
            var storage7 = table.GetStorage<C7>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i], storage3[i],
                    storage4[i], storage5[i], storage6[i], storage7[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query, 
        Action<Entity, C1, C2, C3, C4, C5, C6, C7, C8> action)
    {
        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            var storage5 = table.GetStorage<C5>(Identity.None);
            var storage6 = table.GetStorage<C6>(Identity.None);
            var storage7 = table.GetStorage<C7>(Identity.None);
            var storage8 = table.GetStorage<C8>(Identity.None);

            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(query.World, entities[i]), storage1[i], storage2[i], storage3[i],
                    storage4[i], storage5[i], storage6[i], storage7[i], storage8[i]);
            }

            table.Unlock();
        }

        query.World.ApplyTableOperations();
    }
}