using System;
using Pooong;
using Unity.Entities;

partial class UIUpdateSystem : SystemBase
{
    public int CartedHeartCount;
    public int CartedHeartTarget;
    public int BrokenHeartCount;
    public int BrokenHeartTarget;
    public Action<bool> GameEnded;

    private bool ended = false;

    protected override void OnCreate()
    {
        RequireForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        if (ended) return;

        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.Running)
        {
            CartedHeartCount = gameState.CartedHeartCount;
            CartedHeartTarget = gameState.TargetCartedHeartCount;
            BrokenHeartCount = gameState.BrokenHeartCount;
            BrokenHeartTarget = gameState.TargetBrokenHeartCount;
        }
        else
        {
            GameEnded.Invoke(gameState.MissionAccomplished);
            ended = false;
        }
    }
}