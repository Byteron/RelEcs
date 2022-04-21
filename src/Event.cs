using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public struct Added<T>
    {
        public Entity Entity;
        public Added(Entity entity) => Entity = entity;
    }

    public struct Removed<T>
    {
        public Entity Entity;
        public Removed(Entity entity) => Entity = entity;
    }

    internal struct EventSystemList { public List<Type> List; }
    internal struct EventLifeTime { public int Value; }

    internal class EventLifeTimeSystem : ISystem
    {
        public void Run(Commands commands)
        {
            var query = commands.Query().With<EventLifeTime>();

            query.ForEach((Entity entity, ref EventLifeTime lifeTime) =>
            {
                lifeTime.Value++;

                Console.WriteLine($"Event: {entity.Id.Number} has age {lifeTime.Value}.");

                if (lifeTime.Value == 2)
                {
                    entity.Despawn();
                }
            });
        }
    }
}