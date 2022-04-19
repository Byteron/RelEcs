using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class Mask
    {
        public List<TypeId> TargetRelations { get; private set; } = new List<TypeId>();
        public List<TypeId> SourceRelations { get; private set; } = new List<TypeId>();
        public List<int> AnyRelations { get; private set; } = new List<int>();

        public BitSet IncludeBitSet = new BitSet();
        public BitSet ExcludeBitSet = new BitSet();

        public BitSet AddedBitSet = new BitSet();
        public BitSet RemovedBitSet = new BitSet();

        public void With<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            IncludeBitSet.Set(typeId.Index);
        }

        public void Without<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            ExcludeBitSet.Set(typeId.Index);
        }

        public void Added<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            AddedBitSet.Set(typeId.Index);
        }

        public void Removed<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            RemovedBitSet.Set(typeId.Index);
        }
    }
}