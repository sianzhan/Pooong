using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Pooong
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    partial struct PaddleControlSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PooongConfig>();

            state.EntityManager.AddComponent<PaddleControlData>(state.SystemHandle);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var offset = UpdatePaddleOffset(ref state);
            UpdatePaddlePosition(ref state, offset);
            UpdateCartPosition(ref state, offset);
        }

        [BurstCompile]
        public float UpdatePaddleOffset(ref SystemState state)
        {
            var paddleControlData = SystemAPI.GetComponentRW<PaddleControlData>(state.SystemHandle);
            var playerInputData = SystemAPI.GetSingleton<PlayerInputData>();
            var config = SystemAPI.GetSingleton<PooongConfig>();

            var offset = paddleControlData.ValueRO.OffSet;

            offset += playerInputData.MoveValue.x * config.PlayerMovementInputSensitivity;
            offset = math.clamp(offset, -config.MaxPaddleMovementOffset, config.MaxPaddleMovementOffset);

            paddleControlData.ValueRW.OffSet = offset;

            return paddleControlData.ValueRO.OffSet;
        }

        [BurstCompile]
        public void UpdatePaddlePosition(ref SystemState state, in float offset)
        {
            foreach (var (paddle, transform, physicsVelocity)
            in SystemAPI.Query<RefRO<Paddle>, RefRO<LocalTransform>, RefRW<PhysicsVelocity>>())
            {
                var elevation = offset;
                if (paddle.ValueRO.Side == Paddle.PaddleSide.Left) elevation = -elevation;

                var currentY = transform.ValueRO.Position.y;
                var targetY = paddle.ValueRO.InitialPosition.y + elevation;
                var displacement = targetY - currentY;

                var velocity = displacement / SystemAPI.Time.DeltaTime;

                physicsVelocity.ValueRW.Linear.y = velocity;
            }
        }

        [BurstCompile]
        public void UpdateCartPosition(ref SystemState state, in float offset)
        {
            foreach (var (cart, transform, physicsVelocity)
            in SystemAPI.Query<RefRO<Cart>, RefRO<LocalTransform>, RefRW<PhysicsVelocity>>())
            {
                var currentX = transform.ValueRO.Position.x;
                var targetX = cart.ValueRO.InitialPosition.x + offset;
                var displacement = targetX - currentX;

                var velocity = displacement / SystemAPI.Time.DeltaTime;

                physicsVelocity.ValueRW.Linear.x = velocity;
            }
        }
    }
}