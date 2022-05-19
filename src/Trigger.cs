using System;
using System.Collections.Generic;

namespace RelEcs;

public class Trigger<T>
{
    public T Value;
    public Trigger(T value) => Value = value;
}

internal class TriggerSystemList
{
    public readonly List<Type> List;
    public TriggerSystemList(List<Type> list) => List = list;
}

internal class TriggerLifeTime { public int Value; }

internal class TriggerLifeTimeSystem : ISystem
{
    public void Run(Commands commands)
    {
        var query = commands.Query().Has<TriggerLifeTime>();
        
        query.ForEach((Entity entity, TriggerSystemList systemList, TriggerLifeTime lifeTime) =>
        {
            lifeTime.Value++;
        
            if (lifeTime.Value < 2) return;
            
            ListPool<Type>.Add(systemList.List);
            entity.Despawn();
        });
    }
}