using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class World
    {
        EntityMeta[] entities = new EntityMeta[512];
        int entityCount = 1;

        List<int>[,] relations = new List<int>[512, 512];

        int[] despawnedEntities = new int[512];
        int despawnedEntityCount = 0;

        int[] storageIndices = new int[512];
        IStorage[] storages = new IStorage[512];
        int storageCount = 0;

        public Entity Spawn()
        {

            int id = 0;

            if (despawnedEntityCount > 0)
            {
                id = despawnedEntities[--despawnedEntityCount];
            }
            else
            {
                id = entityCount++;
                Array.Resize(ref entities, entityCount << 1);
                // TODO: figure out how to resize relationMetas
            }

            ref var meta = ref entities[id];

            meta.Id = id;
            meta.Gen += 1;

            if (meta.Bitset == null)
            {
                meta.Bitset = new BitSet();
            }

            return new Entity() { Id = id, Gen = meta.Gen };
        }

        public void Despawn(Entity entity)
        {
            ref var meta = ref entities[entity.Id];

            if (meta.Id == 0)
            {
                return;
            }

            if (meta.Bitset.Count > 0)
            {
                for (int i = 1; i <= storageCount; i++)
                {
                    if (meta.Bitset.Get(storages[i].TypeId))
                    {
                        storages[i].Remove(entity);
                    }
                }
            }

            meta.Id = 0;
            meta.Bitset.ClearAll();

            if (despawnedEntityCount == despawnedEntities.Length)
            {
                Array.Resize(ref despawnedEntities, despawnedEntityCount << 1);
            }

            despawnedEntities[despawnedEntityCount++] = entity.Id;
        }

        public ref T AddComponent<T>(Entity entity) where T : struct, IComponent
        {
            var storage = GetStorage<T>();

            ref var meta = ref entities[entity.Id];
            meta.Bitset.Set(storage.TypeId);

            return ref storage.Add(entity);
        }

        public ref T GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            var storage = GetStorage<T>();
            return ref storage.Get(entity);
        }

        public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
        {
            var storage = GetStorage<T>();

            ref var meta = ref entities[entity.Id];
            meta.Bitset.Clear(storage.TypeId);

            storage.Remove(entity);
        }

        public void AddRelation<T>(Entity entity, Entity target) where T: struct, IRelation
        {
            var storage = GetStorage<Relation<T>>();

            ref var meta = ref entities[entity.Id];
            meta.Bitset.Set(storage.TypeId);

            storage.Add(entity) = new Relation<T>() { Data = new T(), Entity = target };

            var targetId = target.Id;
            var entityId = entity.Id;
            var typeId = storage.TypeId;

            if (relations[targetId, typeId] == null)
            {
                relations[targetId, typeId] = new List<int>();
            }

            relations[targetId, typeId].Add(entityId);
        }

        public ref Relation<T> GetRelation<T>(Entity entity) where T : struct, IRelation
        {
            var storage = GetStorage<Relation<T>>();
            return ref storage.Get(entity);
        }

        public void RemoveRelation<T>(Entity entity) where T: struct, IRelation
        {
            var storage = GetStorage<Relation<T>>();
            var relation = storage.Get(entity);

            ref var meta = ref entities[entity.Id];
            meta.Bitset.Clear(storage.TypeId);

            storage.Remove(entity);

            var targetId = relation.Entity.Id;
            var entityId = entity.Id;
            var typeId = storage.TypeId;

            relations[targetId, typeId]?.Remove(entityId);
        }

        public Entity[] FindRelated<T>(Entity target) where T : struct, IRelation
        {
            if (!IsAlive(target))
            {
                throw new Exception("target id not alive");
            }

            List<Entity> entities = new List<Entity>();

            var typeId = ComponentType<Relation<T>>.Id;

            if (relations[target.Id, typeId] != null)
            {
                foreach (int entity in relations[target.Id, typeId])
                {
                    ref var meta = ref this.entities[entity];

                    entities.Add(new Entity() { Id = entity, Gen = meta.Gen });
                }
            }

            return entities.ToArray();
        }

        internal Entity[] Query(Mask mask)
        {
            List<Entity> entities = new List<Entity>();

            for (var i = 1; i <= entityCount; i++)
            {
                ref var meta = ref this.entities[i];

                if (meta.Id == 0)
                {
                    continue;
                }

                var isRelatedToTarget = true;

                foreach (var pair in mask.RelationMap)
                {
                    var targetId = pair.Key;
                    var typeId = pair.Value;

                    var relatedEntities = relations[targetId, typeId];
                    isRelatedToTarget &= relatedEntities == null ? false : relatedEntities.Contains(i);
                }

                if (!isRelatedToTarget)
                {
                    continue;
                }

                if (mask.IsCompatibleWith(meta.Bitset))
                {
                    entities.Add(new Entity { Id = meta.Id, Gen = meta.Gen });
                }
            }

            return entities.ToArray();
        }

        public Storage<T> GetStorage<T>() where T : struct
        {
            Storage<T> storage = null;

            var typeId = ComponentType<T>.Id;

            if (typeId >= storageIndices.Length)
            {
                Array.Resize(ref storageIndices, typeId << 1);
            }

            ref var storageId = ref storageIndices[typeId];

            if (storageId > 0)
            {
                storage = storages[storageId] as Storage<T>;
            }
            else
            {
                storageId = ++storageCount;
                Array.Resize(ref storages, (storageCount << 1));
                storage = new Storage<T>(typeId);
                storages[storageId] = storage;
            }

            return storage;
        }

        public bool IsAlive(Entity entity)
        {
            return entities[entity.Id].Id > 0;
        }
    }
}