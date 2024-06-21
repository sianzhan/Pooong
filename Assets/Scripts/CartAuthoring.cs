using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pooong
{
    public class CartAuthoring : MonoBehaviour
    {
        public class Baker : Baker<CartAuthoring>
        {
            public override void Bake(CartAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Cart
                {
                    InitialPosition = authoring.transform.position,
                });
            }
        }
    }

    public struct Cart : IComponentData
    {
        public float3 InitialPosition;
    }
}