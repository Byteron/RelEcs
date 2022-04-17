using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    internal sealed class Mask
    {
        internal List<(Entity, int)> Relations { get; private set; } = new List<(Entity, int)>();
        
        internal BitSet IncludeBitSet = new BitSet();
        internal BitSet ExcludeBitSet = new BitSet();


        // BitSet addedBitSet = new BitSet();
        // BitSet removedBitSet = new BitSet();

        internal void With<T>() where T : struct
        {
            var typeId = TypeIdAssigner<T>.Id;
            IncludeBitSet.Set(typeId);
        }

        internal void Without<T>() where T : struct
        {
            var typeId = TypeIdAssigner<T>.Id;
            ExcludeBitSet.Set(typeId);
        }

        internal void IsA<T>() where T : struct
        {
            var typeId = TypeIdAssigner<T>.Id;
            IncludeBitSet.Set(typeId);
        }

        internal void IsA<T>(Entity target) where T : struct
        {
            var typeId = TypeIdAssigner<T>.Id;
            Relations.Add((target, typeId));
        }

        // internal void Added<T>() where T : struct
        // {
        //     var typeId = ComponentType<T>.Id;
        //     addedBitSet.Set(typeId);
        // }

        // internal void Removed<T>() where T : struct
        // {
        //     var typeId = ComponentType<T>.Id;
        //     removedBitSet.Set(typeId);
        // }
    }
}