using System;

namespace Bitron.Ecs
{
    public interface IComponent { }
    public interface IRelation { }

    public interface IStorage
    {
        int TypeId { get; set; }
        object GetRaw(Entity entity);
        bool Has(Entity entity);
        void Remove(Entity entity);
    }

    public sealed class Storage<Component> : IStorage where Component : struct
    {
        public int TypeId { get; set; }

        int[] indices = new int[512];
        Component[] components = new Component[512];
        int componentCount = 0;


        internal Storage(int typeId)
        {
            TypeId = typeId;
        }

        public ref Component Add(Entity entity)
        {
            int index = componentCount++;

            if (componentCount == components.Length)
            {
                Array.Resize(ref components, componentCount << 1);
            }

            indices[entity.Id] = index;
            return ref components[index];
        }

        public ref Component Get(Entity entity)
        {
            return ref components[indices[entity.Id]];
        }

        public object GetRaw(Entity entity)
        {
            return components[indices[entity.Id]];
        }

        public void Remove(Entity entity)
        {
            ref var index = ref indices[entity.Id];

            if (index > 0)
            {
                components[index] = default;
                index = 0;
            }
        }

        public bool Has(Entity entity)
        {
            return indices[entity.Id] > 0;
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

    public struct Relation<T> where T : notnull, IRelation
    {
        public Entity Entity;
        public T Data;
    }
}