# RelEcs
### A lightweight and easy to use entity component system with an effective feature set for making games.

## World

```csharp
// A world is a container for different kinds of data like entities & components.
World world = new World();
```

## Entity

```csharp
// Spawn a new entity into the world.
Entity entity = world.Spawn();

// Despawn an entity.
entity.Despawn();
```

## Component

```csharp
// Components are just plain old data structs.
struct Position { public int X, Y; }
struct Velocity { public int X, Y; }

// Add new components to an entity.
entity.Add<Position>().Add(new Velocity { X = 1, Y = 0 });

// Get a component from an entity.
ref var vel = ref entity.Get<Velocity>();

// Remove a component from an entity.
entity.Remove<Position>();
```

## Element

```csharp
// Elements are unique class-based components that are attached directly to worlds.
class SavePath { string Value; }

// Add an element to the world.
// You can only have one element per type in a world.
world.AddElement(new SavePath( Value = "user://saves/"));

// Get an element from the world.
var savePath = world.GetElement<SavePath>();
Console.WriteLine(savePath.Value);

// Remove an element from the world.
world.RemoveElement<SavePath>();
```

## Relation

```csharp
// Like components, relations are structs.
struct Likes { }
struct Owes { int Amount; }

struct Apples { }

var bob = world.Spawn();
var frank = world.Spawn();

// Relations consist of components, associated with a "target".
// The target can either be another component, or an entity.
bob.Add<Likes>(typeof(Apples));
//   Component ^^^^^^^^^^^^^^

frank.Add(new Owes { Amount = 100 }, bob);
//                            Entity ^^^

// You can test if an entity has a component or a relation.
bool doesBobHaveApples = bob.Has<Apples>();
bool doesBobLikeApples = bob.Has<Likes>(typeof(Apples));

// Or get it directly.
// In this case, we retrieve the amount that Frank owes Bob.
ref var owes = ref frank.Get<Owes>(bob);
Console.WriteLine($"Frank owes Bob {owes.Amount} dollars");
```

## Commands

```csharp
// Commands are a wrapper around World that provide additional helpful functions.
Commands commands = new Commands(world);

// You *do not* need to create your own commands.
// They will be automatically provided for you as the System.Run(Commands) argument.
```

## System

```csharp
// Systems add all the functionality to the Entity Component System.
// Usually, you would run them from within your game loop.
public class MoveSystem : ISystem
{
    public void Run(Commands commands)
    {        
        // Loop over a desired set of components, using ForEach.
        // Beware: Currently only works with components, *not* with relations
        commands.ForEach((ref Position pos, ref Velocity vel) =>
        {
            pos.Value += vel.Value;
        });

        // You can also access the entity within the loop
        commands.ForEach((Entity entity, ref Position pos, ref Velocity vel) =>
        {
            pos.Value += vel.Value;
            // Example: "Tag" a component to show that it has moved.
            entity.Add<Moved>();
        });
    }
}
```


### Running a System

```csharp
// Create an instance of your system.
var moveSystem = new MoveSystem();

// Run the system.
// The system will match all entities of the world you enter as the parameter.
moveSystem.Run(world);

// You can run a system as many times as you like.
moveSystem.Run(world);
moveSystem.Run(world);
moveSystem.Run(world);

// Usually, systems are run once a frame, inside your game loop.
```

## Query

```csharp
// You can create complex, expressive queries.
// Here, we request every entity that has a Name and Age component and owes money to Bob.
var appleLovers = commands.Query()
    .Has<Name, Age>()
    .Has<Owes>(bob);
```

## Triggers

```csharp
// Triggers are also just structs and very similar to components.
// They act much like a simplified, ECS version of C# events.
struct MyTrigger { }

// You can send a bunch of triggers inside of a system.
commands.Send<MyTrigger>();
commands.Send<MyTrigger>();
commands.Send<MyTrigger>();

// In any system, including the origin system, you can now receive these triggers.
commands.Receive((MyTrigger e) =>
{
    Console.WriteLine("It's a trigger!");
});

// Output:
// It's a trigger!
// It's a trigger!
// It's a trigger!

// NOTE: Triggers live until the end of the next frame, to make sure every system receives them.
// Each trigger is always received exactly ONCE per system.
```

## SystemGroup

```csharp
// You can create system groups, which bundle together multiple systems.
SystemGroup group = new SystemGroup();

// Add any amount of systems to the group.
group.Add(new SomeSystem())
     .Add(new SomeOtherSystem())
     .Add(new AThirdSystem());

// Running a system group will run all of its systems in the order they were added.
group.Run(world);
```

## Example of a Game Loop

```csharp
// In this example, we are using the Godot Engine.
using Godot;
using RelEcs;
using World = RelEcs.World; // Godot also has a World class, so we need to specify this.

public class GameLoop : Node
{
    World world = new World();

    SystemGroup initSystems = new SystemGroup();
    SystemGroup runSystems = new SystemGroup();
    SystemGroup cleanupSystems = new SystemGroup();

    // Called once on node construction.
    public GameLoop()
    {
        // Add your initialization systems.
        initSystem.Add(new SomeSpawnSystem());

        // Add systems that should run every frame.
        runSystems.Add(new PhysicsSystem())
            .Add(new AnimationSystem())
            .Add(new PlayerControlSystem());
        
        // Add systems that are called once when the Node is removed.
        cleanupSystems.Add(new DespawnSystem());
    }

    // Called every time the node is added to the scene.
    public override void _Ready()
    {
        // Run the init systems.
        initSystems.Run(world);   
    }

    // Called every frame. Delta is time since the last frame.
    public override void _Process(float delta)
    {
        // Run the run systems.
        runSystems.Run(world);

        // IMPORTANT: For RelEcs to work properly, we need to tell the world when a frame is done.
        // For that, we call Tick() on the world, at the end of the function.
        world.Tick();
    }

    // Called when the node is removed from the SceneTree.
    public override void _ExitTree()
    {
        // Run the cleanup systems.
        cleanupSystems.Run(world);
    }
}
```
