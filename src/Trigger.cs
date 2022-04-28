using System;
using System.Collections.Generic;

namespace RelEcs
{
    public struct Added<T>
    {
        public Entity Entity;
        public Added(Entity entity) => Entity = entity;
    }

    internal struct TriggerSystemList : IReset<TriggerSystemList>
    {
        public List<Type> List;

        public void Reset(ref TriggerSystemList c)
        {
            c.List = new List<Type>();
        }
    }

    internal struct TriggerLifeTime { public int Value; }

    internal class TriggerLifeTimeSystem : ISystem
    {
        public void Run(Commands commands)
        {
            var query = commands.Query().Has<TriggerLifeTime>();

            query.ForEach((Entity entity, ref TriggerLifeTime lifeTime) =>
            {
                lifeTime.Value++;

                if (lifeTime.Value == 2)
                {
                    entity.Despawn();
                }
            });
        }
    }
}