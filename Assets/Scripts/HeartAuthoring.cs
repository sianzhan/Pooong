using Unity.Entities;
using UnityEngine;

namespace Pooong
{
    public class HeartAuthoring : MonoBehaviour
    {
        public bool Breakable = true;
        public GameObject FragileHeartPrefab;

        public class Baker : Baker<HeartAuthoring>
        {
            public override void Bake(HeartAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Heart
                {
                    Breakable = authoring.Breakable,
                    FragileHeartPrefab = GetEntity(authoring.FragileHeartPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct Heart : IComponentData
    {
        public bool Breakable;
        public Entity FragileHeartPrefab;
    }
}