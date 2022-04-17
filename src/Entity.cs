using System;

namespace Bitron.Ecs
{
    public struct Id
    {
        public static Id None = default;
        public static Id Any = new Id(int.MaxValue, 0);

        internal int Number;
        internal int Generation;

        public Id(int id, int gen)
        {
            Number = id;
            Generation = gen;
        }

        public override bool Equals(object obj)
        {
            return (obj is Id other) && Number == other.Number && Generation == other.Generation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Generation);
        }

        public override string ToString()
        {
            return "" + Number;
        }

        public static bool operator ==(Id left, Id right) => left.Equals(right);
        public static bool operator !=(Id left, Id right) => !left.Equals(right);
    }

    public struct Entity
    {
        public static Entity None = default;
        public static Entity Any = new Entity(Id.Any);

        public bool IsAny { get => Id == Id.Any; }
        public bool IsNone { get => Id == Id.None; }
        public bool IsAlive { get => world.IsAlive(Id); }

        internal Id Id { get; }
        private World world;

        public Entity(Id id)
        {
            this.world = null;
            Id = id;
        }

        public Entity(World world, Id id)
        {
            this.world = world;
            Id = id;
        }

        public Entity(World world, int id, int gen)
        {
            this.world = world;
            Id = new Id(id, gen);
        }

        public Entity Add<T>(T data = default) where T : struct
        {
            world.AddComponent<T>(Id, Entity.None) = data;
            return this;
        }

        public Entity Add<T>(Entity target, T data = default) where T : struct
        {
            world.AddComponent<T>(Id, target) = data;
            return this;
        }

        public ref T Get<T>() where T : struct
        {
            return ref world.GetComponent<T>(Id, Entity.None);
        }

        public ref T Get<T>(Entity target) where T : struct
        {
            return ref world.GetComponent<T>(Id, target);
        }

        public bool Has<T>() where T : struct
        {
            return world.HasComponent<T>(Id, Entity.None);
        }

        public bool Has<T>(Entity target) where T : struct
        {
            return world.HasComponent<T>(Id, target);
        }

        public Entity Remove<T>() where T : struct
        {
            world.RemoveComponent<T>(Id, Entity.None);
            return this;
        }

        public Entity Remove<T>(Entity target) where T : struct
        {
            world.RemoveComponent<T>(Id, target);
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
}