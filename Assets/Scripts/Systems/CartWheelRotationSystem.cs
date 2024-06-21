using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pooong
{
    partial struct CartWheelRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PaddleControlData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var angle = -SystemAPI.GetSingleton<PaddleControlData>().OffSet;

            foreach (var transform
            in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<CartWheel>())
            {
                transform.ValueRW.Rotation = quaternion.RotateZ(angle);
            }
        }
    }
}