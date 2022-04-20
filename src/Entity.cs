using System;

namespace Bitron.Ecs
{
    public struct Entity
    {
        public static Entity None = default;
        public static Entity Any = new Entity(EntityId.Any);

        public bool IsAny { get => Id == EntityId.Any; }
        public bool IsNone { get => Id == EntityId.None; }
        public bool IsAlive { get => world.IsAlive(Id); }

        public EntityId Id { get; }
        private World world;

        public Entity(EntityId id)
        {
            this.world = null;
            Id = id;
        }

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

        public Entity Add<T>(bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, EntityId.None, triggerEvent);
            return this;
        }

        public Entity Add<T>(T data, bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, EntityId.None, triggerEvent) = data;
            return this;
        }

        public Entity Add<T>(Entity target, T data = default, bool triggerEvent = false) where T : struct
        {
            world.AddComponent<T>(Id, target.Id, triggerEvent) = data;
            return this;
        }

        public ref T Get<T>() where T : struct
        {
            return ref world.GetComponent<T>(Id);
        }

        public ref T Get<T>(Entity target) where T : struct
        {
            return ref world.GetComponent<T>(Id, target.Id);
        }

        public bool Has<T>() where T : struct
        {
            return world.HasComponent<T>(Id);
        }

        public bool Has<T>(Entity target) where T : struct
        {
            return world.HasComponent<T>(Id, target.Id);
        }

        public Entity Remove<T>(bool triggerEvent = false) where T : struct
        {
            world.RemoveComponent<T>(Id, EntityId.None, triggerEvent);
            return this;
        }

        public Entity Remove<T>(Entity target, bool triggerEvent = false) where T : struct
        {
            world.RemoveComponent<T>(Id, target.Id, triggerEvent);
            return this;
        }

        public void Despawn()
        {
            world.Despawn(Id);
        }

        public override bool Equals(object obj)
        {
            return (obj is Entity entity) && Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

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
            return HashCode.Combine(Number, Generation);
        }

        public override string ToString()
        {
            return "" + Number;
        }

        public static bool operator ==(EntityId left, EntityId right) => left.Equals(right);
        public static bool operator !=(EntityId left, EntityId right) => !left.Equals(right);
    }
}