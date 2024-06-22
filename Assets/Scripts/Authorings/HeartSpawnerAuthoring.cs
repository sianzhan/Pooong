using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace Pooong
{
    public class HeartSpawnerAuthoring : MonoBehaviour
    {
        public GameObject HeartPrefab;

        public class Baker : Baker<HeartSpawnerAuthoring>
        {
            public override void Bake(HeartSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new HeartSpawner
                {
                    HeartPrefab = GetEntity(authoring.HeartPrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }

    public struct HeartSpawner : IComponentData
    {
        public Entity HeartPrefab;
    }
}