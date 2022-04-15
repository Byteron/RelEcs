using System;

namespace Bitron.Ecs
{
    public sealed class Query
    {
        World _world;

        int[] _indices = new int[512];
        Entity[] _entities = new Entity[512];

        int _entityCount = 0;

        internal Query(World world)
        {
            _world = world;
        }

        public void AddEntity(Entity entity)
        {

            if (_entityCount == _entities.Length)
            {
                Array.Resize(ref _entities, _entityCount << 1);
            }
            int index = ++_entityCount;

            _entities[index] = entity;
            _indices[entity.Id] = index;
        }

        public void RemoveEntity(Entity entity)
        {
            var index = _indices[entity.Id];
            _indices[entity.Id] = 0;

            _entityCount--;

            if (index < _entityCount)
            {
                _entities[index] = _entities[_entityCount];
                _indices[_entities[index].Id] = index;
            }
        }
    }

    public sealed class Mask
    {
        BitSet _includeBitSet = new BitSet();
        BitSet _excludeBitSet = new BitSet();

        BitSet _addedBitSet = new BitSet();
        BitSet _removedBitSet = new BitSet();

        public void Include<Component>() where Component : struct
        {
            var typeId = ComponentType<Component>.Id;
            _includeBitSet.Set(typeId);
        }

        public void Exclude<Component>() where Component : struct
        {
            var typeId = ComponentType<Component>.Id;
            _excludeBitSet.Set(typeId);
        }

        public void Added<Component>() where Component : struct
        {
            var typeId = ComponentType<Component>.Id;
            _addedBitSet.Set(typeId);
        }

        public void Removed<Component>() where Component : struct
        {
            var typeId = ComponentType<Component>.Id;
            _removedBitSet.Set(typeId);
        }

        internal bool IsCompatibleWith(BitSet bitSet)
        {
            if (!bitSet.HasAllBitsSet(_includeBitSet))
            {
                return false;
            }

            if (bitSet.HasAnyBitSet(_excludeBitSet))
            {
                return false;
            }

            return true;
        }
    }
}