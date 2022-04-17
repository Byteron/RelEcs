using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class World
    {
        Id[] entities = new Id[512];
        BitSet[] bitsets = new BitSet[512];

        int entityCount = 0;

        List<Id>[,] targetRelations = new List<Id>[512, 512];

        Id[] unusedIds = new Id[512];
        int unusedIdCount = 0;

        int[] componentStorageIndices = new int[512];
        IComponentStorage[] componentStorages = new IComponentStorage[512];
        int componentStorageCount = 0;

        int[] relationStorageIndices = new int[512];
        IRelationStorage[] relationStorages = new IRelationStorage[512];
        int relationStorageCount = 0;

        public Entity Spawn()
        {
            Id id = default;

            if (unusedIdCount > 0)
            {
                id = unusedIds[--unusedIdCount];
            }
            else
            {
                id = new Id(++entityCount, 1);

                if (entities.Length == entityCount)
                {
                    Array.Resize(ref entities, entityCount << 1);
                    Array.Resize(ref bitsets, entityCount << 1);
                }

                entities[id.Number] = id;
            }

            ref var bitset = ref bitsets[id.Number];

            if (bitset == null)
            {
                bitset = new BitSet();
            }

            return new Entity(this, id);
        }

        public void Despawn(Id id)
        {
            if (!IsAlive(entities[id.Number]))
            {
                return;
            }

            var bitset = bitsets[id.Number];

            if (bitset.Count > 0)
            {
                for (int i = 1; i <= componentStorageCount; i++)
                {
                    if (bitset.Get(componentStorages[i].TypeId))
                    {
                        var componentStorage = this.componentStorages[i];
                        componentStorage.Remove(id.Number);
                    }
                }

                for (int i = 1; i <= relationStorageCount; i++)
                {
                    var relationStorage = relationStorages[i];
                    var typeId = relationStorage.TypeId;

                    // remove child relations
                    var children = targetRelations[id.Number, typeId];

                    if (children != null)
                    {
                        foreach (var child in children)
                        {
                            relationStorage.Remove(child.Number);
                            bitsets[child.Number].Clear(typeId);
                        }

                        children.Clear();
                    }

                    if (bitset.Get(typeId))
                    {
                        // remove parent relations
                        var parent = relationStorage.GetEntity(id.Number);
                        var siblings = targetRelations[parent.Id.Number, typeId];
                        siblings.Remove(id);
                        relationStorage.Remove(id.Number);
                    }
                }
            }

            bitset.ClearAll();

            if (unusedIdCount == unusedIds.Length)
            {
                Array.Resize(ref unusedIds, unusedIdCount << 1);
            }


            entities[id.Number].Generation += 1;
            unusedIds[unusedIdCount++] = entities[id.Number];
        }

        public ref T AddComponent<T>(Id id) where T : struct
        {
            var storage = GetComponentStorage<T>();

            var bitset = bitsets[id.Number];
            bitset.Set(storage.TypeId);

            return ref storage.Add(id.Number);
        }

        public ref T GetComponent<T>(Id id) where T : struct
        {
            var storage = GetComponentStorage<T>();
            return ref storage.Get(id.Number);
        }

        public bool HasComponent<T>(Id id) where T : struct
        {
            var storage = GetComponentStorage<T>();
            return storage.Has(id.Number);
        }

        public void RemoveComponent<T>(Id id) where T : struct
        {
            var storage = GetComponentStorage<T>();

            var bitset = bitsets[id.Number];
            bitset.Clear(storage.TypeId);

            storage.Remove(id.Number);
        }

        public void AddRelation<T>(Id id, Entity target, T data = default) where T : struct
        {
            var storage = GetRelationStorage<T>();

            var bitset = bitsets[id.Number];
            bitset.Set(storage.TypeId);

            storage.Add(id.Number) = new Relation<T>() { Entity = target, Data = data };

            var targetId = target.Id.Number;
            var entityId = id.Number;
            var typeId = storage.TypeId;

            if (targetRelations[targetId, typeId] == null)
            {
                targetRelations[targetId, typeId] = new List<Id>();
            }

            targetRelations[targetId, typeId].Add(id);
        }

        public Relation<T> GetRelation<T>(Id id) where T : struct
        {
            var storage = GetRelationStorage<T>();
            return storage.Get(id.Number);
        }

        public void RemoveRelation<T>(Id id) where T : struct
        {
            var storage = GetRelationStorage<T>();
            var relation = storage.Get(id.Number);

            var bitset = bitsets[id.Number];
            bitset.Clear(storage.TypeId);

            storage.Remove(id.Number);

            var targetId = relation.Entity.Id.Number;
            var entityId = id.Number;
            var typeId = storage.TypeId;

            targetRelations[targetId, typeId].Remove(id);
        }

        internal Entity[] Query(Mask mask)
        {
            List<Entity> filteredEntities = new List<Entity>();

            for (var i = 1; i <= entityCount; i++)
            {
                Id id = entities[i];

                if (!IsAlive(id))
                {
                    continue;
                }

                var bitset = bitsets[i];

                if (!bitset.HasAllBitsSet(mask.IncludeBitSet))
                {
                    continue;
                }

                if (bitset.HasAnyBitSet(mask.ExcludeBitSet))
                {
                    continue;
                }

                var isRelatedToTarget = true;

                foreach (var pair in mask.Relations)
                {
                    var target = pair.Item1;
                    var typeId = pair.Item2;

                    var relatedEntities = targetRelations[target.Id.Number, typeId];
                    isRelatedToTarget &= relatedEntities == null ? false : relatedEntities.Contains(id);
                }

                if (!isRelatedToTarget)
                {
                    continue;
                }

                filteredEntities.Add(new Entity(this, id));
            }

            return filteredEntities.ToArray();
        }

        public ComponentStorage<T> GetComponentStorage<T>() where T : struct
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

        public RelationStorage<T> GetRelationStorage<T>() where T : struct
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

        public bool IsAlive(Id id)
        {
            return entities[id.Number].Generation == id.Generation;
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