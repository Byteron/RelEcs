using System;

namespace Bitron.Ecs
{
    // public struct TypeId
    // {
    //     internal Id TargetId;
    //     internal int Id;
    //     internal bool IsPair;

    //     // override object.Equals
    //     public override bool Equals(object obj)
    //     {
    //         if (!(obj is TypeId other) || IsPair != other.IsPair)
    //         {
    //             return false;
    //         }
            
    //         return IsPair ? Id == other.Id && TargetId == other.TargetId : Id == other.Id;
    //     }

    //     public override int GetHashCode()
    //     {
    //         return HashCode.Combine(TargetId, Id);
    //     }

    //     public override string ToString()
    //     {
    //         return base.ToString();
    //     }

    //     public static bool operator ==(TypeId left, TypeId right) => left.Equals(right);
	// 	public static bool operator !=(TypeId left, TypeId right) => !left.Equals(right);
    // }

    public struct Id
    {
        public bool IsNone { get => Number == 0; }

        internal int Number;
        internal ushort Generation;

        public Id(int id, ushort gen)
        {
            Number = id;
            Generation = gen;
        }

        // override object.Equals
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
        internal Id Id { get; }
        private World world;

        public Entity(World world, Id id)
        {
            this.world = world;
            Id = id;
        }

        public Entity(World world, int id, ushort gen)
        {
            this.world = world;
            Id = new Id(id, gen);
        }

        public Entity Add<T>() where T : struct
        {
            world.AddComponent<T>(Id);
            return this;
        }

        public Entity Add<T>(T data) where T : struct
        {
            world.AddComponent<T>(Id) = data;
            return this;
        }

        public Entity Add<T>(Entity target) where T : struct
        {
            world.AddRelation<T>(Id, target);
            return this;
        }

        public ref T Get<T>() where T : struct
        {
            return ref world.GetComponent<T>(Id);
        }

        public ref T Get<T>(Entity target) where T : struct
        {
            return ref world.GetComponent<T>(Id);
        }

        public bool Has<T>() where T : struct
        {
            return world.HasComponent<T>(Id);
        }

        public Entity Remove<T>() where T : struct
        {
            world.RemoveComponent<T>(Id);
            return this;
        }

        public void Despawn()
        {
            world.Despawn(Id);
        }

        public bool IsAlive(Entity entity)
        {
            return world.IsAlive(Id);
        }

        // override object.Equals
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
    }
}