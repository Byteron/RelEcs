# RelEcs
## A lightweight and easy to use entity component system with an effective feature set for making games.

## World

```csharp
// This creates a new world, which serves as a container for entities.
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
bob.Add<Likes, Apples>();
//   Component ^^^^^^

frank.Add(new Owes { Amount = 100 }, bob);
//                            Entity ^^^

// You can test if an entity has a component or a relation.
bool doesBobHaveApples = bob.Has<Apples>();
bool doesBobLikeApples = bob.Has<Likes, Apples>();

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

## Query

```csharp
// every entity that has a Name and Age component and owes bob money.
var appleLovers = commands.Query()
    .Has<Name, Age>()
    .Has<Owes>(bob);
```

Running a System

```csharp
// create an instance of our system
var moveSystem = new MoveSystem();

// run the system
// the system will apply to all entities of that world
moveSystem.Run(world);

// we can run it however often you want
// usually systems are run in a game loop
moveSystem.Run(world);
moveSystem.Run(world);
moveSystem.Run(world);
```

## Triggers

```csharp
// triggers are again just structs. They are basically components internally
struct MyTrigger { }

// send a bunch of triggers
commands.Send<MyTrigger>();
commands.Send<MyTrigger>();
commands.Send<MyTrigger>();


// in any system, you can now receive triggers
commands.Receive((MyTrigger e) =>
{
    Console.WriteLine("A Trigger!");
});
// note that triggers only live for 2 frames and can only be received ONCE per SYSTEM

// Output:
// A Trigger!
// A Trigger!
// A Trigger!
```

## Build-in Triggers

```csharp
var entity = commands.Spawn();

// normally you add components like this. No triggers are spawned by default
entity.Add<Name>(new Name("Walter"));

// you can pass in an optional parameter 'spawnTrigger' 
// to spawn an Added<T> trigger
entity.Add<Old>(true);


// you can receive those build-in triggers like your custom triggers as well
commands.Receive((Added<Old> addedTrigger) =>
{
    Console.WriteLine("Old component added to " + addedTrigger.Entity);
})
```

## Build-in Components

```csharp
struct Person { }

var entity = commands.Spawn();

// we have a build-in tag component `IsA` that we can use for relations
entity.IsA<Person>(); // same as: entity.Add<IsA, Person>();

// and of course we can also query for it with a special build-in method
var query = commands.Query().IsA<Person>(); // same as: Query().Has<IsA, Person>()
```

## SystemGroup

```csharp
// create a new system group
SystemGroup group = new SystemGroup();

// add any amount of systems to a system group
group.Add(new SomeSystem())
    .Add(new SomeOtherSystem())
    .Add(new AThirdSystem());

// running a system group will run all added systems in the order you added them
group.Run(world);
```

## Game Loop

```csharp
// using Godot as an Example
using Godot;
using RelEcs;

public class GameLoopNode : Node
{
    // Godot has a World class
    RelEcs.World world = new RelEcs.World();

    SystemGroup initSystems = new SystemGroup();
    SystemGroup runSystems = new SystemGroup();
    SystemGroup cleanupSystems = new SystemGroup();

    // called once when the node is being initialized
    public override void _Init()
    {
        initSystem.Add(new SomeSpawnSystem());

        // add systems that run every loop
        runSystems.Add(new PhysicsSystem())
            .Add(new AnimationSystem())
            .Add(new PlayerControlSystem());
        
        // add systems that are called once in the end
        cleanupSystems.Add(new DespawnSystem());
    }

    // is called once after the Node has been added to the SceneTree
    public override void _Ready()
    {
        // add your init systems

        // run init systems
        initSystems.Run(world);   
    }

    // is called once every frame
    public override void _Process(float delta)
    {
        // run systems 
        runSystems.Run(world);

        // IMPORTANT: For our ecs events to work properly, we need to tell the world
        // when a frame is done. For that, we call Tick() on the world
        world.Tick();
    }

    // called once when the node is removed from the SceneTree
    public override void _ExitTree()
    {
        // run cleanup systems
        cleanupSystems.Run(world);
    }
}
```
