using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public abstract class System
{
    internal World World;
    
    public abstract void Run();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Spawn()
    {
        return new EntityBuilder(World, World.Spawn().Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Amend(Entity entity)
    {
        return new EntityBuilder(World, entity.Identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn(Entity entity)
    {
        World.Despawn(entity.Identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAlive(Entity entity)
    {
        return entity != null && World.IsAlive(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity) where T : class
    {
        return World.GetComponent<T>(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity, Entity target) where T : class
    {
        return World.GetComponent<T>(entity.Identity, target.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity, Type type) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        return World.GetComponent<T>(entity.Identity, typeIdentity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, out T component) where T : class
    {
        return TryGetComponent(entity, RelEcs.Entity.None, out component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, Entity target, out T component) where T : class
    {
        if (World.HasComponent<T>(entity.Identity, target.Identity))
        {
            component = World.GetComponent<T>(entity.Identity, target.Identity);
            return true;
        }

        component = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, Type type, out T component) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        if (World.HasComponent<T>(entity.Identity, typeIdentity))
        {
            component = World.GetComponent<T>(entity.Identity, typeIdentity);
            return true;
        }

        component = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity) where T : class
    {
        return World.HasComponent<T>(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity, Entity target) where T : class
    {
        return World.HasComponent<T>(entity.Identity, target.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity, Type type) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        return World.HasComponent<T>(entity.Identity, typeIdentity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity[] GetTargets<T>(Entity entity) where T : class
    {
        return World.GetTargets<T>(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Send<T>(T trigger) where T : class
    {
        World.Send(trigger);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Receive<T>(Action<T> action) where T : class
    {
        World.Receive(this, action);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddElement<T>(T element) where T : class
    {
        World.AddElement(element);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReplaceElement<T>(T element) where T : class
    {
        World.ReplaceElement(element);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddOrReplaceElement<T>(T element) where T : class
    {
        if (World.HasElement<T>()) World.ReplaceElement(element);
        else World.AddElement(element);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetElement<T>() where T : class
    {
        return World.GetElement<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetElement<T>(out T element) where T : class
    {
        if (World.HasElement<T>())
        {
            element = World.GetElement<T>();
            return true;
        }

        element = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasElement<T>() where T : class
    {
        return World.HasElement<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveElement<T>() where T : class
    {
        World.RemoveElement<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<Entity> Query()
    {
        return new QueryBuilder<Entity>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C> Query<C>()
        where C : class
    {
        return new QueryBuilder<C>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2> Query<C1, C2>()
        where C1 : class
        where C2 : class
    {
        return new QueryBuilder<C1, C2>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3> Query<C1, C2, C3>()
        where C1 : class
        where C2 : class
        where C3 : class
    {
        return new QueryBuilder<C1, C2, C3>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3, C4> Query<C1, C2, C3, C4>()
        where C1 : class
        where C2 : class
        where C3 : class
        where C4 : class
    {
        return new QueryBuilder<C1, C2, C3, C4>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>()
        where C1 : class
        where C2 : class
        where C3 : class
        where C4 : class
        where C5 : class
    {
        return new QueryBuilder<C1, C2, C3, C4, C5>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>()
        where C1 : class
        where C2 : class
        where C3 : class
        where C4 : class
        where C5 : class
        where C6 : class
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>()
        where C1 : class
        where C2 : class
        where C3 : class
        where C4 : class
        where C5 : class
        where C6 : class
        where C7 : class
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>()
        where C1 : class
        where C2 : class
        where C3 : class
        where C4 : class
        where C5 : class
        where C6 : class
        where C7 : class
        where C8 : class
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>(World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Query<C1, C2, C3, C4, C5, C6, C7, C8, C9>()
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
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>(World);
    }
}

public sealed class SystemGroup
{
    readonly List<System> systems = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SystemGroup Add(System system)
    {
        systems.Add(system);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Run(World world)
    {
        foreach (var system in systems)
        {
            system.Run(world);
        }
    }
}