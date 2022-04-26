using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public struct Entity
    {
        public static Entity None = default;
        public static Entity Any = new Entity(null, EntityId.Any);

        public bool IsAny => Id == EntityId.Any;
        public bool IsNone => Id == EntityId.None;
        public bool IsAlive => world != null && world.IsAlive(Id);

        public EntityId Id { get; }

        World world;

        public Entity(World world, EntityId id)
        {
            this.world = world;
            Id = id;
        }

        public Entity(World world, int id, ushort gen)
        {
            this.world = world;
            Id = new EntityId(id, gen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, EntityId.None, triggerEvent);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T data, bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, EntityId.None, triggerEvent) = data;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(Entity target, bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, target.Id, triggerEvent);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T, TT>(bool triggerEvent = false) where T : struct where TT : struct
        {
            Entity typeEntity = world.GetTypeEntity<TT>();
            world.AddComponent<T>(Id, typeEntity.Id, triggerEvent);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(Entity target, T data, bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, target.Id, triggerEvent) = data;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref world.GetComponent<T>(Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(Entity target) where T : struct
        {
            return ref world.GetComponent<T>(Id, target.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return world.HasComponent<T>(Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>(Entity target) where T : struct
        {
            return world.HasComponent<T>(Id, target.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Remove<T>(bool triggerEvent = false) where T : struct
        {
            world.RemoveComponent<T>(Id, EntityId.None, triggerEvent);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Remove<T>(Entity target, bool triggerEvent = false) where T : struct
        {
            world.RemoveComponent<T>(Id, target.Id, triggerEvent);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Despawn()
        {
            world.Despawn(Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return (obj is Entity entity) && Id.Equals(entity.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Id.ToString();
        }

        public static bool operator ==(Entity left, Entity right) => left.Equals(right);
        public static bool operator !=(Entity left, Entity right) => !left.Equals(right);
    }

    public struct EntityId
    {
        public static EntityId None = default;
        public static EntityId Any = new EntityId(int.MaxValue, 0);

        public int Number;
        public ushort Generation;

        public EntityId(int id, ushort gen)
        {
            Number = id;
            Generation = gen;
        }

        public override bool Equals(object obj)
        {
            return (obj is EntityId other) && Number == other.Number && Generation == other.Generation;
        }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                int hashcode = 1430287;
                hashcode = hashcode * 7302013 ^ Number.GetHashCode();
                hashcode = hashcode * 7302013 ^ Generation.GetHashCode();
                return hashcode;
            }
        }

        public override string ToString()
        {
            return "" + Number;
        }

        public static bool operator ==(EntityId left, EntityId right) => left.Equals(right);
        public static bool operator !=(EntityId left, EntityId right) => !left.Equals(right);
    }
}