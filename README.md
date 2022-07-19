# RelEcs
### A lightweight and easy to use entity component system with an effective feature set for making games.

## Components

```csharp
// Components are simple classes.
class Position { public int X, Y; }
class Velocity { public int X, Y; }
```

## Systems

```csharp
// Systems add all the functionality to the Entity Component System.
// Usually, you would run them from within your game loop.
public class MoveSystem : ASystem
{
    public override void Run()
    {
        // iterate sets of components.
        foreach(var (pos, vel) in Query<Position, Velocity>())
        {
            pos.X += vel.X;
            pos.Y += vel.Y;
        }
    }
}
```

### Spawning / Despawning Entities

```csharp
public override void Run()
{
    // Spawn a new entity into the world and store the id for later use
    Entity entity = Spawn().Id();
    
    // Despawn an entity.
    Despawn(entity);
}
```

### Adding / Removing Components

```csharp
public override void Run()
{
    // Spawn an entity with components
    Entity entity = Spawn()
        .Add<Position>()
        .Add(new Velocity { X = 5 })
        .Add<Tag>()
        .Id();
    
    // Change an Entities Components
    On(entity).Add(new Name { Value = "Bob" }).Remove<Tag>();
}
```

### Relations

```csharp
// Like components, relations are classes.
class Apples { }
class Likes { }
class Owes { public int Amount; }
```

```csharp
public override void Run()
{
    var bob = Spawn().Id();
    var frank = Spawn().Id();
    
    // Relations consist of components, associated with a "target".
    // The target can either be another component, or an entity.
    On(bob).Add<Likes>(typeof(Apples));
    //   Component     ^^^^^^^^^^^^^^
    
    On(frank).Add(new Owes { Amount = 100 }, bob);
    //                                Entity ^^^
    
    // if you want to know if an entity has a component
    bool doesBobHaveApples = HasComponent<Apples>(bob);
    // if you want to know if an entity has a relation
    bool doesBobLikeApples = HasComponent<Likes>(bob, typeof(Apples));
    
    // Or get it directly.
    // In this case, we retrieve the amount that Frank owes Bob.
    var owes = GetComponent<Owes>(frank, bob);
    Console.WriteLine($"Frank owes Bob {owes.Amount} dollars");
}
```

### Queries

```csharp
public override void Run()
{
    // With queries, we can get a list of components that we can iterate through.
    // A simple query looks like this
    var query = Query<Position, Velocity>();
    
    // Now we can loop through these components
    foreach(var (pos, vel) in query)
    {
        pos.Value += vel.Value;
    }
            
    // You can create more complex, expressive queries through the QueryBuilder.
    // Here, we request every entity that has a Name component, owes money to Bob and does not have the Dead tag.
    var appleLovers = QueryBuilder<Entity, Name>().Has<Owes>(bob).Not<Dead>().Build();
    
    // Note that we only get the components inside Query<>.
    // Has<T>, Not<T> and Any<T> only filter, but we don't actually get T int he loop.
    foreach(var (entity, name) in query)
    {
        Console.WriteLine($"Entity {entity} with name {name.Value} owes bob money and is still alive.")
    }
}
```

### Triggers

```csharp
// Triggers are also just classes and very similar to components.
// They act much like a simplified, ECS version of C# events.
class MyTrigger { }
```

```csharp
public override void Run()
{
    // You can send a bunch of triggers inside of a system.
    Send(new MyTrigger());
    Send(new MyTrigger());
    Send(new MyTrigger());
    
    // In any system, including the origin system, you can now receive these triggers.
    foreach (var t in Receive<T>())
    {
        Console.WriteLine("It's a trigger!");
    }
    
    // Output:
    // It's a trigger!
    // It's a trigger!
    // It's a trigger!
    
    // NOTE: Triggers live until the end of the next frame, to make sure every system receives them.
    // Each trigger is always received exactly ONCE per system.
}
```

## Creating a World

```csharp
// A world is a container for different kinds of data like entities & components.
World world = new World();
```

## Running a System

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

## SystemGroups

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
