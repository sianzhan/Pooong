using Unity.Burst;
using Unity.Entities;
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
            state.RequireForUpdate<PooongConfig>();

            queryCartedHeart = SystemAPI.QueryBuilder().WithAll<Heart, CartItem>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var stageConfigs = SystemAPI.GetSingletonBuffer<PooongStageConfig>();
            var gameState = SystemAPI.GetSingletonRW<GameState>();
            var countHeartCarted = queryCartedHeart.CalculateEntityCount();

            if (gameState.ValueRO.CurrentStage + 1 >= stageConfigs.Length)
            {
                // End Game
                return;
            }

            if (countHeartCarted >= stageConfigs[gameState.ValueRO.CurrentStage + 1].RequiredHearts)
            {
                gameState.ValueRW.AmountSpawnedThisStage = 0;
                gameState.ValueRW.CurrentStage += 1;
            }
        }
    }
}