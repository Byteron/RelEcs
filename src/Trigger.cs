using System;
using System.Collections.Generic;

namespace RelEcs;

public class Trigger<T>
{
    public readonly T Value;

    public Trigger()
    {
    }

    public Trigger(T value) => Value = value;
}

internal class TriggerSystemList
{
    public readonly List<Type> List;

    public TriggerSystemList() => List = new List<Type>();
    public TriggerSystemList(List<Type> list) => List = list;
}

internal class TriggerLifeTime
{
    public int Value;
}

internal class TriggerLifeTimeSystem : System
{
    public override void Run()
    {
        var query = Query<Entity, TriggerSystemList, TriggerLifeTime>().Build();
        
        foreach (var (entity, systemList, lifeTime) in query)
        {
            lifeTime.Value++;

            if (lifeTime.Value < 2) return;

            ListPool<Type>.Add(systemList.List);

            Despawn(entity);
        }
    }
}