namespace Bitron.Ecs
{
    public struct Entity
    {
        public int Id;
        public int Gen;
    }

    internal struct EntityMeta
    {
        internal int Id;
        internal int Gen;
        internal BitSet BitSet;
    }
}