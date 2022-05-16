using System.Runtime.CompilerServices;

namespace RelEcs;

public static class CommandsExtensions
{
    public delegate void RefAction<C>(ref C c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C>(this Commands commands, RefAction<C> action)
        where C : struct
    {
        var query = commands.Query().Has<C>();

        foreach (var table in query.Tables)
        {
            var storage = table.GetStorage<C>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(ref storage[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }

    public delegate void RefAction<C1, C2>(ref C1 c1, ref C2 c2);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2>(this Commands commands, RefAction<C1, C2> action)
        where C1 : struct
        where C2 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>();

        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(ref storage1[i], ref storage2[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void RefAction<C1, C2, C3>(ref C1 c1, ref C2 c2, ref C3 c3);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3>(this Commands commands, RefAction<C1, C2, C3> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>();

        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(ref storage1[i], ref storage2[i], ref storage3[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }

    public delegate void RefAction<C1, C2, C3, C4>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4>(this Commands commands, RefAction<C1, C2, C3, C4> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>();
    
        foreach (var table in query.Tables)
        {
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            var storage4 = table.GetStorage<C4>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void RefAction<C1, C2, C3, C4, C5>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5>(this Commands commands, RefAction<C1, C2, C3, C4, C5> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();
    
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
                action(ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void RefAction<C1, C2, C3, C4, C5, C6>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6>(this Commands commands, RefAction<C1, C2, C3, C4, C5, C6> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();
    
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
                action(ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i], ref storage6[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void RefAction<C1, C2, C3, C4, C5, C6, C7>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this Commands commands, RefAction<C1, C2, C3, C4, C5, C6, C7> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();
    
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
                action(ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i], ref storage6[i], ref storage7[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void RefAction<C1, C2, C3, C4, C5, C6, C7, C8>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7, ref C8 c8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands, RefAction<C1, C2, C3, C4, C5, C6, C7, C8> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
        where C8 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();

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
                action(ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i], ref storage6[i], ref storage7[i], ref storage8[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction(Entity entity);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach(this Commands commands, EntityRefAction action)
    {
        var query = commands.Query();

        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(commands.World, entities[i]));
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }

    public delegate void EntityRefAction<C>(Entity entity, ref C c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C>(this Commands commands, EntityRefAction<C> action)
        where C : struct
    {
        var query = commands.Query().Has<C>();

        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage = table.GetStorage<C>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(commands.World, entities[i]), ref storage[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2>(Entity entity, ref C1 c1, ref C2 c2);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2>(this Commands commands, EntityRefAction<C1, C2> action)
        where C1 : struct
        where C2 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>();

        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2, C3>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3>(this Commands commands, EntityRefAction<C1, C2, C3> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>();

        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage1 = table.GetStorage<C1>(Identity.None);
            var storage2 = table.GetStorage<C2>(Identity.None);
            var storage3 = table.GetStorage<C3>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i], ref storage3[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2, C3, C4>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4>(this Commands commands, EntityRefAction<C1, C2, C3, C4> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>();
    
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
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2, C3, C4, C5>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();
    
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
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5, C6> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();
    
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
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i], ref storage6[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6, C7>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5, C6, C7> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();
    
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
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i], ref storage6[i], ref storage7[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6, C7, C8>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7, ref C8 c8);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5, C6, C7, C8> action)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
        where C8 : struct
    {
        var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();
    
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
                action(new Entity(commands.World, entities[i]), ref storage1[i], ref storage2[i], ref storage3[i], ref storage4[i], ref storage5[i], ref storage6[i], ref storage7[i], ref storage8[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
}