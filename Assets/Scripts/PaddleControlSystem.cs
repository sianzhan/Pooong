using Pooong;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;

partial struct PaddleControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Paddle>();

        state.EntityManager.AddComponent<PaddleControlData>(state.SystemHandle);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var paddleControlData = SystemAPI.GetComponentRW<PaddleControlData>(state.SystemHandle);
        var playerInputData = SystemAPI.GetSingleton<PlayerInputData>();

        var offset = paddleControlData.ValueRO.OffSet;

        offset += playerInputData.MoveValue.x * 0.1f;
        offset = math.clamp(offset, -10, 10);

        paddleControlData.ValueRW.OffSet = offset;

        foreach (var (paddle, transform)
        in SystemAPI.Query<RefRO<Paddle>, RefRW<LocalTransform>>())
        {
            transform.ValueRW.Position.y = paddle.ValueRO.Side == Paddle.PaddleSide.Right ? offset : -offset;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
