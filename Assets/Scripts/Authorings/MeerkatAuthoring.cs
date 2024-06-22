using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pooong
{
    public class MeerkatAuthoring : MonoBehaviour
    {
        public class Baker : Baker<MeerkatAuthoring>
        {
            public override void Bake(MeerkatAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.NonUniformScale);

                AddComponent(entity, new Meerkat { InitialPosition = authoring.transform.position });
            }
        }
    }

    public struct Meerkat : IComponentData
    {
        public float3 InitialPosition;
    }
}