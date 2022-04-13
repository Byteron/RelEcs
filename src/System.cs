using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class EcsSystemGroup
    {
        private List<ISystem> _systems = new List<ISystem>();

        public EcsSystemGroup Add(ISystem system)
        {
            _systems.Add(system);
            return this;
        }

        // public EcsSystemGroup OneFrame<Component>() where Component : struct
        // {
        //     return Add(new RemoveAllComponentsOfType<Component>());
        // }

        public void Run(Commands commands)
        {
            for(var i = 0; i < _systems.Count; i++)
            {
                _systems[i].Run(commands);
            }
        }
    }

    public interface ISystem
    {
        void Run(Commands commands);
    }

    // public class RemoveAllComponentsOfType<Component> : ISystem where Component : struct
    // {
    //     public void Run(World world)
    //     {
    //         var query = world.Query<Component>().End();
    //         var pool = world.GetPool<Component>();

    //         foreach (var entity in query)
    //         {
    //             pool.Remove(entity);
    //         }
    //     }
    // }
}
