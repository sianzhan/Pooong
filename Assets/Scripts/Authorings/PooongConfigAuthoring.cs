using Unity.Entities;
using UnityEngine;

namespace Pooong
{
    public class PooongConfigAuthoring : MonoBehaviour
    {
        public int MaxConcurrentHeartAmount = 5;
        public float MaxPaddleMovementOffset = 5f;
        public float PlayerMovementInputSensitivity = 0.1f;
        public float GravityFactorInCart = 20f;
        public float SlowFactor = 0.5f;

        public class Baker : Baker<PooongConfigAuthoring>
        {
            public override void Bake(PooongConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new PooongConfig
                {
                    MaxConcurrentHeartAmount = authoring.MaxConcurrentHeartAmount,
                    MaxPaddleMovementOffset = authoring.MaxPaddleMovementOffset,
                    PlayerMovementInputSensitivity = authoring.PlayerMovementInputSensitivity,
                    GravityFactorInCart = authoring.GravityFactorInCart,
                    SlowFactor = authoring.SlowFactor
                });
            }
        }
    }

    public struct PooongConfig : IComponentData
    {
        public int MaxConcurrentHeartAmount;
        public float MaxPaddleMovementOffset;
        public float PlayerMovementInputSensitivity;
        public float GravityFactorInCart;
        public float SlowFactor;
    }
}