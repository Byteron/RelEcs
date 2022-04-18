# BitEcs

### World
```csharp
// create a new world
World world = new World();
```

### Entity
```csharp
// spawn a new entity
Entity entity = world.Spawn();

// despawn an entity
entity.Despawn();
```

### Component
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
### Resource
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
### Relation
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
### Commands
```csharp
// Commands is a Wrapper around World that provides
// additional helpful functions
Commands commands = new Commands(world);
```
### Query
```csharp
// every entity that has a Name component and owes bob money.
Entity{} appleLovers = commands.Query()
    .With<Name>()
    .With<Owes>(bob)
    .Apply();
```
### System
```csharp
public class MoveSystem : ISystem
{
    public void Run(Commands commands)
    {
        // every entity that has a Position and Velocity component
        Entity[] movers = commands.Query()
            .With<Position>()
            .With<Velocity>()
            .Apply();
        
        foreach (var mover in movers)
        {
            ref var pos = ref mover.Get<Position>();
            ref var vel = ref mover.Ger<Velocity>();

            pos.Value += vel.Value;
        }
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