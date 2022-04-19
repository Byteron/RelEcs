using System.Collections.Generic;

namespace Bitron.Ecs
{
    internal sealed class Mask
    {
        internal List<TypeId> TargetRelations { get; private set; } = new List<TypeId>();
        internal List<TypeId> SourceRelations { get; private set; } = new List<TypeId>();
        internal List<int> AnyRelations { get; private set; } = new List<int>();

        internal BitSet IncludeBitSet = new BitSet();
        internal BitSet ExcludeBitSet = new BitSet();

        internal BitSet AddedBitSet = new BitSet();
        internal BitSet RemovedBitSet = new BitSet();

        internal void With<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            IncludeBitSet.Set(typeId.Index);
        }

        internal void Without<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            ExcludeBitSet.Set(typeId.Index);
        }

        internal void Added<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            AddedBitSet.Set(typeId.Index);
        }

        internal void Removed<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            RemovedBitSet.Set(typeId.Index);
        }
    }
}