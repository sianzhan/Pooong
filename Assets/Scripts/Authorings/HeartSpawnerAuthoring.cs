using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace Pooong
{
    public class HeartSpawnerAuthoring : MonoBehaviour
    {
        public GameObject HeartPrefab;
        public float SpawnRadius;
        public float InitialVelocityMin;
        public float InitialVelocityMax;

        public class Baker : Baker<HeartSpawnerAuthoring>
        {
            public override void Bake(HeartSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new HeartSpawner
                {
                    HeartPrefab = GetEntity(authoring.HeartPrefab, TransformUsageFlags.Dynamic),
                    SpawnRadius = authoring.SpawnRadius,
                    InitialVelocityMin = authoring.InitialVelocityMin,
                    InitialVelocityMax = authoring.InitialVelocityMax
                });
            }
        }
    }

    public struct HeartSpawner : IComponentData
    {
        public Entity HeartPrefab;
        public float SpawnRadius;
        public float InitialVelocityMin;
        public float InitialVelocityMax;
    }
}