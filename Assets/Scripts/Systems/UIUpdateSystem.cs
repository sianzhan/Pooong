using Pooong;
using Unity.Burst;
using Unity.Entities;

partial class UIUpdateSystem : SystemBase
{
    public int CartedHeartCount;
    public int CartedHeartTarget;
    public int BrokenHeartCount;
    public int BrokenHeartTarget;

    [BurstCompile]
    protected override void OnCreate()
    {
        RequireForUpdate<GameState>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        CartedHeartCount = gameState.CartedHeartCount;
        CartedHeartTarget = gameState.TargetCartedHeartCount;
        BrokenHeartCount = gameState.BrokenHeartCount;
        BrokenHeartTarget = gameState.TargetBrokenHeartCount;

    }
}
