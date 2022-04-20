using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class Mask
    {
        private World world;

        public List<long> TargetRelations { get; private set; } = new List<long>();
        public List<long> SourceRelations { get; private set; } = new List<long>();
        public List<int> AnyRelations { get; private set; } = new List<int>();

        public BitSet IncludeBitSet = new BitSet();
        public BitSet ExcludeBitSet = new BitSet();

        public BitSet AddedBitSet = new BitSet();
        public BitSet RemovedBitSet = new BitSet();

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

        public void Added<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);
            AddedBitSet.Set(index);
        }

        public void Removed<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Value<T>(target.Id.Number);
            var index = world.GetStorageIndex(typeId);
            RemovedBitSet.Set(index);
        }
    }
}