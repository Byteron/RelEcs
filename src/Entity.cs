using System.Collections.Generic;

namespace Bitron.Ecs
{
    public struct Entity
    {
        public int Id;
        public int Gen;
    }

    internal struct EntityMeta
    {
        internal Entity Entity;
        internal BitSet BitSet;
    }
}