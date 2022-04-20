using System.Linq;

namespace Bitron.Ecs
{
    public sealed class Query
    {
        public World World;
        public Entity[] Entities;
        Mask mask;

        public Query(World world, Mask mask, Entity[] entities)
        {
            World = world;
            Entities = entities;
            this.mask = mask;
        }
    }

    public sealed class Mask
    {
        private World world;

        public BitSet IncludeBitSet = new BitSet();
        public BitSet ExcludeBitSet = new BitSet();

        public Mask(World world)
        {
            this.world = world;
        }
        public void With<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);
            IncludeBitSet.Set(index);
        }

        public void Without<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);
            ExcludeBitSet.Set(index);
        }
    }
}