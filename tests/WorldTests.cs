using System;
using System.Linq;
using RelEcs;
using Xunit;

namespace RelEcs.Tests
{
    public class WorldTests
    {
        private sealed class Position
        {
            public float X;
            public float Y;
        }

        private sealed class Velocity
        {
            public float X;
            public float Y;
        }

        private sealed class TestTrigger
        {
            public string Message = string.Empty;
        }

        private sealed class TestTriggerSystem : ITriggerSystem<TestTrigger>
        {
            public int RunCount;
            public string LastMessage = string.Empty;

            public void Run(World world, TestTrigger trigger)
            {
                RunCount++;
                LastMessage = trigger.Message;
            }
        }

        [Fact]
        public void Spawn_CreatesAliveEntity()
        {
            var world = new World();

            var entity = world.Spawn().Id();

            Assert.True(world.IsAlive(entity));
            Assert.False(entity.IsNone);
            Assert.False(entity.IsAny);
        }

        [Fact]
        public void Despawn_MakesEntityNotAlive()
        {
            var world = new World();
            var entity = world.Spawn().Id();

            world.Despawn(entity);

            Assert.False(world.IsAlive(entity));
        }

        [Fact]
        public void AddAndGetComponent_WorksForSimpleComponent()
        {
            var world = new World();
            var entity = world.Spawn().Id();

            world.AddComponent(entity, new Position { X = 1, Y = 2 });

            Assert.True(world.HasComponent<Position>(entity));

            var pos = world.GetComponent<Position>(entity);
            Assert.Equal(1, pos.X);
            Assert.Equal(2, pos.Y);
        }

        [Fact]
        public void TryGetComponent_ReturnsFalseWhenMissing()
        {
            var world = new World();
            var entity = world.Spawn().Id();

            var result = world.TryGetComponent<Position>(entity, out var pos);

            Assert.False(result);
            Assert.Null(pos);
        }

        [Fact]
        public void RemoveComponent_RemovesExistingComponent()
        {
            var world = new World();
            var entity = world.Spawn().Add<Position>().Id();

            Assert.True(world.HasComponent<Position>(entity));

            world.RemoveComponent<Position>(entity);

            Assert.False(world.HasComponent<Position>(entity));
        }

        [Fact]
        public void DespawnAllWith_RemovesAllEntitiesWithComponent()
        {
            var world = new World();

            var entityWith = world.Spawn().Add<Position>().Id();
            var entityWithout = world.Spawn().Id();

            world.DespawnAllWith<Position>();

            Assert.False(world.IsAlive(entityWith));
            Assert.True(world.IsAlive(entityWithout));
        }

        [Fact]
        public void Query_ReturnsMatchingEntities()
        {
            var world = new World();

            var a = world.Spawn().Add<Position>().Id();
            var b = world.Spawn().Add<Position>().Add<Velocity>().Id();
            var c = world.Spawn().Add<Velocity>().Id();

            var query = world.Query<Entity>().Has<Position>().Build();

            var result = new System.Collections.Generic.List<Entity>();
            foreach (var entity in query)
            {
                result.Add(entity);
            }

            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e == a);
            Assert.Contains(result, e => e == b);
            Assert.DoesNotContain(result, e => e == c);
        }

        [Fact]
        public void WorldInfo_IsUpdatedOnTick()
        {
            var world = new World();

            var e1 = world.Spawn().Id();
            var e2 = world.Spawn().Id();
            world.AddElement(new object());

            world.Tick();

            var info = world.Info;

            Assert.True(info.EntityCount >= 2);
            Assert.True(info.AllocatedEntityCount >= info.EntityCount);
            Assert.True(info.ArchetypeCount >= 1);
            Assert.True(info.ElementCount >= 1);
        }

        [Fact]
        public void Send_RegistersAndRunsTriggerSystems()
        {
            var world = new World();
            var system = new TestTriggerSystem();
            world.RegisterTriggerSystem(system);

            world.Send(new TestTrigger { Message = "hello" });
            world.RunTriggerSystems();

            Assert.Equal(1, system.RunCount);
            Assert.Equal("hello", system.LastMessage);

            // Ensure trigger entity is cleaned up by lifetime system
            world.Tick();
            world.Tick();
        }
    }
}
