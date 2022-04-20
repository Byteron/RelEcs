# BitEcs
## An easy to use entity component system with and effective feature set for making games.

## World

```csharp
// create a new world
World world = new World();
```

## Entity

```csharp
// spawn a new entity
Entity entity = world.Spawn();

// despawn an entity
entity.Despawn();
```

## Component

```csharp
// components are plain ol data structs
struct Position { int X, Y; }
struct Velocity { int X, Y; }

// add new components to an entity
entity.Add<Position>().Add(new Velocity { X = 1, Y = 0 });

// get a component from an entity
ref var vel = ref entity.Get<Velocity>();

// remove a component from an entity;
entity.Remove<Position>();
```

## Resource

```csharp
// resources are classes
class SavePath { string Value; }

// add a resource to the world
// you can only have one resource per type in the world
world.AddResource(new SavePath( Value = "user://saves/"));

// get a resource from the world
var savePath = world.GetResource<MyResource>();
Console.WriteLine(savePath.Value);

// remove a resource from the world
world.RemoveResource<MyResource>();
```

## Relation

```csharp
// like components, relations are structs
struct Likes { }
struct Owes { int Value; }

var apples = world.Spawn();

var bob = world.Spawn();
var frank = world.Spawn();

// relations basically are just components, but also
// associated with a second "target" entity
bob.Add<Likes>(apples);
//             ^^^^^^
frank.Add(new Owes { Value = 100 }, bob);
//                                  ^^^

// we can ask if an entity has a component or relation
bool doesBobLikeApples = bob.Has<Likes>(apples);

// or get it directly
ref var owes = ref frank.Get<Owes>(bob);
Console.WriteLine($"Frank owes Bob {owes.Value} dollars");
```

## Commands

```csharp
// Commands is a Wrapper around World that provides
// additional helpful functions
Commands commands = new Commands(world);
```

## Query

```csharp
// every entity that has a Name component and owes bob money.
Entity{} appleLovers = commands.Query()
    .With<Name>()
    .With<Owes>(bob)
    .Apply();
```

## System

```csharp
public class MoveSystem : ISystem
{
    public void Run(Commands commands)
    {
        // every entity that has a Position and Velocity component
        Query movers = commands.Query()
            .With<Position>()
            .With<Velocity>()
            .Apply();
        
        // iterate using ForEach.
        // currently only works with Components *not* with relations
        movers.ForEach((ref Position pos, ref Velocity vel) =>
        {
            pos.Value += vel.Value;
        });

        // iterate using ForEach, also gets Entity
        movers.ForEach((Entity entity, ref Position pos, ref Velocity vel) =>
        {
            pos.Value += vel.Value;
            // entity.Add<Moved>(); // tag component to mark that it has moved or something
        });
    }
}
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

## Events

```csharp
// events are again just structs. They are basically components internally
struct MyEvent { }

// send a bunch of events
commands.Send<MyEvent>();
commands.Send<MyEvent>();
commands.Send<MyEvent>();


// in any system, you can now receive events
commands.Receive((MyEvent e) =>
{
    Console.WriteLine("An Event!");
})

// Output:
// "An Event!"
// "An Event!"
// "An Event!"
```

## Build-in Events

```csharp
var entity = commands.Spawn();

// normally you add components like this. No events are spawned by default.
entity.Add<Name>(new Name("Walter"));
entity.Remove<Name>();

// you can pass in an optional parameter 'triggerEvent' 
// to spawn an Added<T> or Removed<T> event.
entity.Add<Old>(true);
entity.Remove<Young>(true);


// you can receive those build-in events like your custom events as well
commands.Receive((Added<Old> addedEvent) =>
{
    Console.WriteLine("Old component added to " + addedEvent.Entity);
})

// same for the removed component
commands.Receive((Removed<Young> removedEvent) =>
{
    Console.WriteLine("Young component removed from " + removedEvent.Entity);
})
```