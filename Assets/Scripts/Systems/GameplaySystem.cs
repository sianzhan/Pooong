using Unity.Burst;
using Unity.Entities;

namespace Pooong
{
    partial struct GameplaySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.AddComponent<GameState>(state.SystemHandle);

            state.EntityManager.SetComponentData<GameState>(state.SystemHandle, new GameState
            {
                Running = true,
                CurrentStage = 0,
                TargetCartedHeartCount = 999,
                TargetBrokenHeartCount = 999
            });

            state.RequireForUpdate<PooongConfig>();
            state.RequireForUpdate<PooongStageConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<PooongConfig>();
            var stageConfigs = SystemAPI.GetSingletonBuffer<PooongStageConfig>();
            var gameState = SystemAPI.GetSingletonRW<GameState>();

            if (!gameState.ValueRO.Running) return;

            if (gameState.ValueRO.CartedHeartCount >= gameState.ValueRO.TargetCartedHeartCount)
            {
                gameState.ValueRW.MissionAccomplished = true;
                gameState.ValueRW.Running = false;
            }
            else if (gameState.ValueRO.BrokenHeartCount >= gameState.ValueRO.TargetBrokenHeartCount)
            {
                gameState.ValueRW.MissionAccomplished = false;
                gameState.ValueRW.Running = false;
            }

            if (!gameState.ValueRO.Running) return;
            if (gameState.ValueRO.CurrentStage + 1 >= stageConfigs.Length) return;

            if (gameState.ValueRO.CartedHeartCount >= stageConfigs[gameState.ValueRO.CurrentStage + 1].RequiredHearts)
            {
                GoToNextStage(gameState, config, stageConfigs[gameState.ValueRO.CurrentStage + 1]);
            }
        }

        public void GoToNextStage(RefRW<GameState> gameState, PooongConfig config, PooongStageConfig stageConfig)
        {
            gameState.ValueRW.AmountSpawnedThisStage = 0;
            gameState.ValueRW.CurrentStage += 1;
            gameState.ValueRW.TargetCartedHeartCount = config.TargetHeartCount;
            gameState.ValueRW.TargetBrokenHeartCount = stageConfig.Lives;
        }
    }
}