using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class World
    {
        EntityMeta[] entities = new EntityMeta[512];
        int entityCount = 0;

        List<Entity>[,] targetRelations = new List<Entity>[512, 512];

        int[] despawnedEntities = new int[512];
        int despawnedEntityCount = 0;

        int[] componentStorageIndices = new int[512];
        IComponentStorage[] componentStorages = new IComponentStorage[512];
        int componentStorageCount = 0;

        int[] relationStorageIndices = new int[512];
        IRelationStorage[] relationStorages = new IRelationStorage[512];
        int relationStorageCount = 0;

        public Entity Spawn()
        {

            int id = 0;

            if (despawnedEntityCount > 0)
            {
                id = despawnedEntities[--despawnedEntityCount];
            }
            else
            {
                id = ++entityCount;
                Array.Resize(ref entities, entityCount << 1);
                // TODO: figure out how to resize relationMetas
            }

            ref var meta = ref entities[id];

            meta.Entity.Id = id;
            meta.Entity.Gen += 1;

            if (meta.BitSet == null)
            {
                meta.BitSet = new BitSet();
            }

            return new Entity() { Id = id, Gen = meta.Entity.Gen };
        }

        public void Despawn(Entity entity)
        {
            ref var meta = ref entities[entity.Id];

            if (meta.Entity.Id == 0)
            {
                return;
            }

            if (meta.BitSet.Count > 0)
            {
                for (int i = 1; i <= componentStorageCount; i++)
                {
                    if (meta.BitSet.Get(componentStorages[i].TypeId))
                    {
                        var componentStorage = componentStorages[i];
                        componentStorage.Remove(entity.Id);
                    }
                }

                for (int i = 1; i <= relationStorageCount; i++)
                {
                    var relationStorage = relationStorages[i];
                    var typeId = relationStorage.TypeId;

                    // remove child relations
                    var children = targetRelations[entity.Id, typeId];
                    
                    if (children != null)
                    {
                        foreach(var child in children)
                        {
                            relationStorage.Remove(child.Id);
                            entities[child.Id].BitSet.Clear(typeId);
                        }
                        
                        children.Clear();
                    }
                    

                    if (meta.BitSet.Get(typeId))
                    {
                        // remove parent relations
                        var parent = relationStorage.GetEntity(entity.Id);
                        var siblings = targetRelations[parent.Id, typeId];
                        siblings.Remove(entity);
                        relationStorage.Remove(entity.Id);
                    }
                }
            }

            meta.Entity.Id = 0;
            meta.BitSet.ClearAll();

            if (despawnedEntityCount == despawnedEntities.Length)
            {
                Array.Resize(ref despawnedEntities, despawnedEntityCount << 1);
            }

            despawnedEntities[despawnedEntityCount++] = entity.Id;
        }

        public ref T AddComponent<T>(Entity entity) where T : struct, IComponent
        {
            var storage = GetComponentStorage<T>();

            ref var meta = ref entities[entity.Id];
            meta.BitSet.Set(storage.TypeId);

            return ref storage.Add(entity.Id);
        }

        public ref T GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            var storage = GetComponentStorage<T>();
            return ref storage.Get(entity.Id);
        }

        public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
        {
            var storage = GetComponentStorage<T>();

            ref var meta = ref entities[entity.Id];
            meta.BitSet.Clear(storage.TypeId);

            storage.Remove(entity.Id);
        }

        public void AddRelation<T>(Entity entity, Entity target, T data = default) where T : struct, IRelation
        {
            var storage = GetRelationStorage<T>();

            ref var meta = ref entities[entity.Id];
            meta.BitSet.Set(storage.TypeId);

            storage.Add(entity.Id) = new Relation<T>() { Entity = target, Data = data };

            var targetId = target.Id;
            var entityId = entity.Id;
            var typeId = storage.TypeId;

            if (targetRelations[targetId, typeId] == null)
            {
                targetRelations[targetId, typeId] = new List<Entity>();
            }

            targetRelations[targetId, typeId].Add(entity);
        }

        public Relation<T> GetRelation<T>(Entity entity) where T : struct, IRelation
        {
            var storage = GetRelationStorage<T>();
            return storage.Get(entity.Id);
        }

        public void RemoveRelation<T>(Entity entity) where T : struct, IRelation
        {
            var storage = GetRelationStorage<T>();
            var relation = storage.Get(entity.Id);

            ref var meta = ref entities[entity.Id];
            meta.BitSet.Clear(storage.TypeId);

            storage.Remove(entity.Id);

            var targetId = relation.Entity.Id;
            var entityId = entity.Id;
            var typeId = storage.TypeId;

            targetRelations[targetId, typeId].Remove(entity);
        }

        internal Entity[] Query(Mask mask)
        {
            List<Entity> entities = new List<Entity>();

            for (var i = 1; i <= entityCount; i++)
            {
                ref var meta = ref this.entities[i];

                if (meta.Entity.Id == 0)
                {
                    continue;
                }

                if (!meta.BitSet.HasAllBitsSet(mask.IncludeBitSet))
                {
                    continue;
                }

                if (meta.BitSet.HasAnyBitSet(mask.ExcludeBitSet))
                {
                    continue;
                }

                var isRelatedToTarget = true;

                foreach (var pair in mask.Relations)
                {
                    var target = pair.Item1;
                    var typeId = pair.Item2;

                    var relatedEntities = targetRelations[target.Id, typeId];
                    isRelatedToTarget &= relatedEntities == null ? false : relatedEntities.Contains(meta.Entity);
                }

                if (!isRelatedToTarget)
                {
                    continue;
                }

                entities.Add(new Entity { Id = meta.Entity.Id, Gen = meta.Entity.Gen });
            }

            return entities.ToArray();
        }

        public ComponentStorage<T> GetComponentStorage<T>() where T : struct, IComponent
        {
            ComponentStorage<T> storage = null;

            var typeId = TypeIdAssigner<T>.Id;

            if (typeId >= componentStorageIndices.Length)
            {
                Array.Resize(ref componentStorageIndices, typeId << 1);
            }

            ref var storageId = ref componentStorageIndices[typeId];

            if (storageId > 0)
            {
                storage = componentStorages[storageId] as ComponentStorage<T>;
            }
            else
            {
                storageId = ++componentStorageCount;
                Array.Resize(ref componentStorages, (componentStorageCount << 1));
                storage = new ComponentStorage<T>(typeId);
                componentStorages[storageId] = storage;
            }

            return storage;
        }

        public RelationStorage<T> GetRelationStorage<T>() where T : struct, IRelation
        {
            RelationStorage<T> storage = null;

            var typeId = TypeIdAssigner<T>.Id;

            if (typeId >= relationStorageIndices.Length)
            {
                Array.Resize(ref relationStorageIndices, typeId << 1);
            }

            ref var storageId = ref relationStorageIndices[typeId];

            if (storageId > 0)
            {
                storage = relationStorages[storageId] as RelationStorage<T>;
            }
            else
            {
                storageId = ++relationStorageCount;
                Array.Resize(ref relationStorages, (relationStorageCount << 1));
                storage = new RelationStorage<T>(typeId);
                relationStorages[storageId] = storage;
            }

            return storage;
        }

        public bool IsAlive(Entity entity)
        {
            return entities[entity.Id].Entity.Id > 0;
        }
    }

    internal class TypeIdAssigner
    {
        protected static int counter = 1;
    }

    internal class TypeIdAssigner<T> : TypeIdAssigner where T : struct
    {
        internal static readonly int Id;
        static TypeIdAssigner() => Id = counter++;
    }
}