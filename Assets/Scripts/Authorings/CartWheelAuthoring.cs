using Unity.Entities;
using UnityEngine;

namespace Pooong
{
    public class CartWheelAuthoring : MonoBehaviour
    {
        public class Baker : Baker<CartWheelAuthoring>
        {
            public override void Bake(CartWheelAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new CartWheel());
            }
        }
    }

    public struct CartWheel : IComponentData { }
}