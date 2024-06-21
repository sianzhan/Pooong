using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.GraphicsIntegration;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting;

// This system is intended as a workaround for the rigidbody constraint of the Unity Physics which is not working.

namespace Pooong
{
    [UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
    partial struct RigidbodyConstraintTransformCacheSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (constraint, transform)
            in SystemAPI.Query<RefRW<RigidbodyConstraint>, RefRO<LocalTransform>>())
            {
                constraint.ValueRW.PositionCache = transform.ValueRO.Position;
                constraint.ValueRW.RotationCache = transform.ValueRO.Rotation;
            }
        }
    }

    [UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
    partial struct RigidbodyConstraintSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (constraint, velocity, transform)
            in SystemAPI.Query<RefRO<RigidbodyConstraint>, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>())
            {
                for (var i = 0; i < 3; ++i)
                {
                    if (constraint.ValueRO.FreezePosition[i])
                    {
                        velocity.ValueRW.Linear[i] = 0;
                        transform.ValueRW.Position[i] = constraint.ValueRO.PositionCache[i];
                    }

                    if (constraint.ValueRO.FreezeRotation[i])
                    {
                        velocity.ValueRW.Angular[i] = 0;
                        transform.ValueRW.Rotation.value[i] = constraint.ValueRO.RotationCache.value[i];
                    }
                }
            }
        }
    }
}