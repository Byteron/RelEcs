using RelEcs;
using Xunit;

namespace RelEcs.Tests
{
    public class SystemGroupTests
    {
        private sealed class TriggerA { }

        private sealed class TriggerB { }

        private sealed class CountingSystem : ISystem
        {
            public int RunCount;

            public void Run(World world)
            {
                RunCount++;
            }
        }

        private sealed class SystemSendingTriggerA : ISystem
        {
            public void Run(World world)
            {
                world.Send(new TriggerA());
            }
        }

        private sealed class SystemSendingTriggerB : ISystem
        {
            public void Run(World world)
            {
                world.Send(new TriggerB());
            }
        }

        private sealed class SystemSendingTriggerAAndB : ISystem
        {
            public void Run(World world)
            {
                world.Send(new TriggerA());
                world.Send(new TriggerB());
            }
        }

        private sealed class TriggerSystemA : ITriggerSystem<TriggerA>
        {
            public int RunCount;

            public void Run(World world, TriggerA trigger)
            {
                RunCount++;
            }
        }

        private sealed class TriggerSystemB : ITriggerSystem<TriggerB>
        {
            public int RunCount;

            public void Run(World world, TriggerB trigger)
            {
                RunCount++;
            }
        }

        [Fact]
        public void SystemGroup_RunsAllSystems()
        {
            var world = new World();
            var systemA = new CountingSystem();
            var systemB = new CountingSystem();

            var group = new SystemGroup()
                .Add(systemA)
                .Add(systemB);

            group.Run(world);

            Assert.Equal(1, systemA.RunCount);
            Assert.Equal(1, systemB.RunCount);
        }

        [Fact]
        public void SystemGroup_RunsRegisteredTriggerSystemsBetweenSystems()
        {
            var world = new World();

            var triggerSystemA = new TriggerSystemA();
            var triggerSystemB = new TriggerSystemB();

            world.RegisterTriggerSystem(triggerSystemA);
            world.RegisterTriggerSystem(triggerSystemB);

            var group = new SystemGroup()
                .Add(new SystemSendingTriggerA())       // should cause TriggerSystemA to run once
                .Add(new SystemSendingTriggerB())       // should cause TriggerSystemB to run once
                .Add(new SystemSendingTriggerAAndB());  // should cause TriggerSystemA and TriggerSystemB to each run once more

            group.Run(world);

            Assert.Equal(2, triggerSystemA.RunCount);
            Assert.Equal(2, triggerSystemB.RunCount);
        }
    }
}
