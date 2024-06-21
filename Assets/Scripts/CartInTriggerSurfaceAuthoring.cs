using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pooong
{
    public class CartInTriggerSurfaceAuthoring : MonoBehaviour
    {
        public class Baker : Baker<CartInTriggerSurfaceAuthoring>
        {
            public override void Bake(CartInTriggerSurfaceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CartInTriggerSurface());
            }
        }
    }

    public struct CartInTriggerSurface : IComponentData { }
}