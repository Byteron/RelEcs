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
        public int TypeId { get; set; }

        int[] _indices = new int[512];
        Component[] _components = new Component[512];
        int _componentCount = 0;

        internal Storage(int typeId)
        {
            TypeId = typeId;
        }

        public ref Component Add(Entity entity)
        {
            int index = _componentCount++;

            if (_componentCount == _components.Length)
            {
                Array.Resize(ref _components, _componentCount << 1);
            }

            _indices[entity.Id] = index;
            return ref _components[index];
        }

        public ref Component Get(Entity entity)
        {
            return ref _components[_indices[entity.Id]];
        }

        public void Remove(Entity entity)
        {
            ref var index = ref _indices[entity.Id];

            if (index > 0)
            {
                _components[index] = default;
                index = 0;
            }
        }

        public bool Has(Entity entity)
        {
            return _indices[entity.Id] > 0;
        }
    }

    internal class ComponentType
    {
        protected static int counter = 1;
    }

    internal class ComponentType<T> : ComponentType where T : struct
    {
        internal static readonly int Id;

        static ComponentType() => Id = counter++;
    }
}