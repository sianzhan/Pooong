using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pooong
{
    public class MeerkatAuthoring : MonoBehaviour
    {
        public int AnimationFPS = 6;

        [SerializeField] public Sprite[] MoveAnims;
        public class Baker : Baker<MeerkatAuthoring>
        {
            public override void Bake(MeerkatAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.NonUniformScale);

                AddComponent(entity, new Meerkat
                {
                    InitialPosition = authoring.transform.position,
                });

                AddComponentObject(entity, new MeerkatAnimation
                {
                    MoveAnims = authoring.MoveAnims,
                    IndexMoveCurrent = 0,
                    LastAnimationTime = 0,
                    Interval = 1 / (double) authoring.AnimationFPS
                });
            }
        }
    }

    public struct Meerkat : IComponentData
    {
        public float3 InitialPosition;
    }

    public class MeerkatAnimation : IComponentData
    {
        public Sprite[] MoveAnims;
        public int IndexMoveCurrent;
        public double LastAnimationTime;
        public double Interval;
    }
}