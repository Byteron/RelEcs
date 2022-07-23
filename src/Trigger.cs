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

internal class SystemList
{
    public readonly List<Type> List;
    public SystemList() => List = ListPool<Type>.Get();
}

internal class LifeTime
{
    public int Value;
}

internal class TriggerLifeTimeSystem : System
{
    public override void Run()
    {
        var query = Query<Entity, SystemList, LifeTime>();
        foreach (var (entity, systemList, lifeTime) in query)
        {
            lifeTime.Value++;
            
            if (lifeTime.Value < 2) return;

            ListPool<Type>.Add(systemList.List);
            Despawn(entity);
        }
    }
}