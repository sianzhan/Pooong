using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using UnityEngine.InputSystem.LowLevel;

namespace Pooong
{
    partial struct GameplaySystem : ISystem
    {
        EntityQuery queryCartedHeart;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.AddComponent<GameState>(state.SystemHandle);

            var entity = state.EntityManager.CreateEntity();

            state.RequireForUpdate<PooongConfig>();
            state.RequireForUpdate<PooongStageConfig>();

            queryCartedHeart = SystemAPI.QueryBuilder().WithAll<Heart, CartItem>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<PooongConfig>();
            var stageConfigs = SystemAPI.GetSingletonBuffer<PooongStageConfig>();
            var gameState = SystemAPI.GetSingletonRW<GameState>();
            var countHeartCarted = queryCartedHeart.CalculateEntityCount();

            if (gameState.ValueRO.CurrentStage + 1 >= stageConfigs.Length)
            {
                if (queryCartedHeart.CalculateEntityCount() >= config.TargetHeartCount)
                {
                    // End game
                }

                return;
            }

            if (countHeartCarted >= stageConfigs[gameState.ValueRO.CurrentStage + 1].RequiredHearts)
            {
                GoToNextStage(gameState, config, stageConfigs[gameState.ValueRO.CurrentStage + 1]);
            }
        }

        public void GoToNextStage(RefRW<GameState> gameState, PooongConfig config, PooongStageConfig stageConfig)
        {
            gameState.ValueRW.AmountSpawnedThisStage = 0;
            gameState.ValueRW.CurrentStage += 1;
            gameState.ValueRW.TargetCartedHeartCount = config.TargetHeartCount;
            gameState.ValueRW.TargetBrokenHeartCount = stageConfig.AvailableHearts;
        }
    }
}