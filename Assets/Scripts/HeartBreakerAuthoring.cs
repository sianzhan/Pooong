using Unity.Entities;
using UnityEngine;

namespace Pooong
{
    public class HeartBreakerAuthoring : MonoBehaviour
    {
        public class Baker : Baker<HeartBreakerAuthoring>
        {
            public override void Bake(HeartBreakerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new HeartBreaker());
            }
        }
    }
}