using System;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class Entity
{
    public static Entity None = default;
    public static Entity Any = new(null, Identity.Any);

    public bool IsAny => Identity == Identity.Any;
    public bool IsNone => Identity == Identity.None;
    public bool IsAlive => world != null && world.IsAlive(Identity);

    public Identity Identity { get; }

    readonly World world;

    public Entity(World world, Identity identity)
    {
        this.world = world;
        Identity = identity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Add<T>(Entity target = default) where T : class, new()
    {
        world.AddComponent<T>(Identity, default, target?.Identity ?? Identity.None);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Add<T>(Type type) where T : class, new()
    {
        var identity = world.GetTypeIdentity(type);
        world.AddComponent<T>(Identity, default, identity);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Add<T>(T data) where T : class
    {
        world.AddComponent(Identity, data);
        return this;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Add<T>(T data, Entity target) where T : class
    {
        world.AddComponent(Identity, data, target.Identity);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Add<T>(T data, Type type) where T : class
    {
        var identity = world.GetTypeIdentity(type);
        world.AddComponent(Identity, data, identity);
        return this;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>() where T : class
    {
        return world.GetComponent<T>(Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>(Entity target) where T : class
    {
        return world.GetComponent<T>(Identity, target.Identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>(Type type) where T : class
    {
        var identity = world.GetTypeIdentity(type);
        return world.GetComponent<T>(Identity, identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(out T component) where T : class
    {
        return TryGet(None, out component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(Entity target, out T component) where T : class
    {
        if (world.HasComponent<T>(Identity))
        {
            component = world.GetComponent<T>(Identity, target.Identity);
            return true;
        }

        component = null;
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(Type type, out T component) where T : class
    {
        var identity = world.GetTypeIdentity(type);
        if (Has<T>())
        {
            component = world.GetComponent<T>(Identity, identity);
            return true;
        }

        component = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>() where T : class
    {
        return world.HasComponent<T>(Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(Entity target) where T : class
    {
        return world.HasComponent<T>(Identity, target.Identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(Type type) where T : class
    {
        var identity = world.GetTypeIdentity(type);
        return world.HasComponent<T>(Identity, identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Remove<T>() where T : class
    {
        world.RemoveComponent<T>(Identity);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Remove<T>(Entity target) where T : class
    {
        world.RemoveComponent<T>(Identity, target.Identity);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Remove<T>(Type type) where T : class
    {
        var identity = world.GetTypeIdentity(type);
        world.RemoveComponent<T>(Identity, identity);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity[] GetTargets<T>() where T : class
    {
        return world.GetTargets<T>(Identity);
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn()
    {
        world.Despawn(Identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
    {
        return (obj is Entity entity) && Identity.Equals(entity.Identity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Identity.ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Entity left, Entity right) => left is not null && left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Entity left, Entity right) => left is null || !left.Equals(right);
}