using System.Runtime.CompilerServices;

namespace RelEcs
{
    public readonly struct Entity
    {
        public static Entity None = default;
        public static Entity Any = new Entity(null, Identity.Any);

        public bool IsAny => Identity == Identity.Any;
        public bool IsNone => Identity == Identity.None;
        public bool IsAlive => world != null && world.IsAlive(Identity);

        public Identity Identity { get; }

        readonly World world;

        public Entity(World world, Identity id)
        {
            this.world = world;
            Identity = id;
        }

        public Entity(World world, int id, ushort gen)
        {
            this.world = world;
            Identity = new Identity(id, gen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(bool spawnTrigger = false) where T : struct
        {
            world.AddComponent<T>(Identity, Identity.None, spawnTrigger);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T data, bool spawnTrigger = false) where T : struct
        {
            world.AddComponent<T>(Identity, Identity.None, spawnTrigger) = data;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(Entity target, bool spawnTrigger = false) where T : struct
        {
            world.AddComponent<T>(Identity, target.Identity, spawnTrigger);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T, TT>(bool spawnTrigger = false) where T : struct where TT : struct
        {
            Entity typeEntity = world.GetTypeEntity<TT>();
            world.AddComponent<T>(Identity, typeEntity.Identity, spawnTrigger);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(Entity target, T data, bool spawnTrigger = false) where T : struct
        {
            world.AddComponent<T>(Identity, target.Identity, spawnTrigger) = data;
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity IsA<TT>(bool spawnTrigger = false) where TT : struct
        {
            Entity typeEntity = world.GetTypeEntity<TT>();
            world.AddComponent<IsA>(Identity, typeEntity.Identity, spawnTrigger);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref world.GetComponent<T>(Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(Entity target) where T : struct
        {
            return ref world.GetComponent<T>(Identity, target.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return world.HasComponent<T>(Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>(Entity target) where T : struct
        {
            return world.HasComponent<T>(Identity, target.Identity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Remove<T>() where T : struct
        {
            world.RemoveComponent<T>(Identity, Identity.None);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Remove<T>(Entity target) where T : struct
        {
            world.RemoveComponent<T>(Identity, target.Identity);
            return this;
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

        public static bool operator ==(Entity left, Entity right) => left.Equals(right);
        public static bool operator !=(Entity left, Entity right) => !left.Equals(right);
    }

    public struct Identity
    {
        public static Identity None = default;
        public static Identity Any = new Identity(int.MaxValue, 0);

        public readonly int Id;
        public ushort Generation;

        public Identity(int id, ushort gen)
        {
            Id = id;
            Generation = gen;
        }

        public override bool Equals(object obj)
        {
            return (obj is Identity other) && Id == other.Id && Generation == other.Generation;
        }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                int hashcode = 1430287;
                hashcode = hashcode * 7302013 ^ Id.GetHashCode();
                hashcode = hashcode * 7302013 ^ Generation.GetHashCode();
                return hashcode;
            }
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public static bool operator ==(Identity left, Identity right) => left.Equals(right);
        public static bool operator !=(Identity left, Identity right) => !left.Equals(right);
    }
}