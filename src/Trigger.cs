using System;
using System.Collections.Generic;

namespace RelEcs;

public struct Added<T>
{
    public Entity Entity;
    public Added(Entity entity) => Entity = entity;
}

public struct Trigger<T> where T : struct
{
    public T Value;
    public Trigger(T value) => Value = value;
}

internal struct TriggerSystemList
{
    public readonly List<Type> List;
    public TriggerSystemList(List<Type> list) => List = list;
}

internal struct TriggerLifeTime { public int Value; }

internal class TriggerLifeTimeSystem : ISystem
{
    public void Run(Commands commands)
    {
        var query = commands.Query().Has<TriggerLifeTime>();

        query.ForEach((Entity entity, ref TriggerSystemList systemList, ref TriggerLifeTime lifeTime) =>
        {
            lifeTime.Value++;

            if (lifeTime.Value < 2) return;
            
            ListPool<Type>.Add(systemList.List);
            entity.Despawn();
        });
    }
}