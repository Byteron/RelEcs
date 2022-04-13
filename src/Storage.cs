using System;

namespace Bitron.Ecs
{
    public interface IStorage
    {
        bool Has(Entity entity);
        void Remove(Entity entity);
    }

    public sealed class Storage<Component> : IStorage where Component : struct
    {
        private int[] _indicies = new int[512];
        private Component[] _components = new Component[512];
        private int _componentCount = 0;

        public ref Component Add(Entity entity)
        {
            int index = _componentCount++;

            if (_componentCount == _components.Length)
            {
                Array.Resize(ref _components, _componentCount << 1);
            }

            _indicies[entity.Id] = index;
            return ref _components[index];
        }

        public void Remove(Entity entity)
        {
            ref var index = ref _indicies[entity.Id];

            if (index > 0)
            {
                _components[index] = default;
                index = 0;
            }
        }

        public bool Has(Entity entity)
        {
            return _indicies[entity.Id] > 0;
        }
    }
}