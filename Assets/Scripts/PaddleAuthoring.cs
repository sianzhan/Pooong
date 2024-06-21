using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pooong
{
    public class PaddleAuthoring : MonoBehaviour
    {
        public Paddle.PaddleSide Side;

        public class Baker : Baker<PaddleAuthoring>
        {
            public override void Bake(PaddleAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Paddle
                {
                    InitialPosition = authoring.transform.position,
                    Side = authoring.Side
                });
            }
        }
    }

    public struct Paddle : IComponentData
    {
        public enum PaddleSide
        {
            Left,
            Right
        }

        public float3 InitialPosition;
        public PaddleSide Side;
    }
}