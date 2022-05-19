using System;
using System.Runtime.CompilerServices;

namespace RelEcs;

public static class CommandsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C>(this Commands commands, Action<C> action)
    {
        var query = commands.Query().Has<C>();

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
        
        commands.World.ApplyTableOperations();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2>(this Commands commands, Action<C1, C2> action)
    {
        var query = commands.Query().Has<C1>().Has<C2>();

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
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3>(this Commands commands, Action<C1, C2, C3> action)
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
                action(storage1[i], storage2[i], storage3[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4>(this Commands commands, Action<C1, C2, C3, C4> action)
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
                action(storage1[i], storage2[i], storage3[i], storage4[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5>(this Commands commands, Action<C1, C2, C3, C4, C5> action)
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
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6>(this Commands commands, Action<C1, C2, C3, C4, C5, C6> action)
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
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i], storage6[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this Commands commands, Action<C1, C2, C3, C4, C5, C6, C7> action)
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
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i], storage6[i], storage7[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands, Action<C1, C2, C3, C4, C5, C6, C7, C8> action)
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
                action(storage1[i], storage2[i], storage3[i], storage4[i], storage5[i], storage6[i], storage7[i], storage8[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach(this Commands commands, Action<Entity> action)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C>(this Commands commands, Action<Entity, C> action)
    {
        var query = commands.Query().Has<C>();

        foreach (var table in query.Tables)
        {
            var entities = table.Entities;
            var storage = table.GetStorage<C>(Identity.None);
            
            table.Lock();
            for (var i = 0; i < table.Count; i++)
            {
                action(new Entity(commands.World, entities[i]), storage[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2>(this Commands commands, Action<Entity, C1, C2> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3>(this Commands commands, Action<Entity, C1, C2, C3> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i], storage3[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4>(this Commands commands, Action<Entity, C1, C2, C3, C4> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i], storage3[i], storage4[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5>(this Commands commands, Action<Entity, C1, C2, C3, C4, C5> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i], storage3[i], storage4[i], storage5[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6>(this Commands commands, Action<Entity, C1, C2, C3, C4, C5, C6> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i], storage3[i], storage4[i], storage5[i], storage6[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this Commands commands, Action<Entity, C1, C2, C3, C4, C5, C6, C7> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i], storage3[i], storage4[i], storage5[i], storage6[i], storage7[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands, Action<Entity, C1, C2, C3, C4, C5, C6, C7, C8> action)
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
                action(new Entity(commands.World, entities[i]), storage1[i], storage2[i], storage3[i], storage4[i], storage5[i], storage6[i], storage7[i], storage8[i]);
            }
            table.Unlock();
        }
        
        commands.World.ApplyTableOperations();
    }
}