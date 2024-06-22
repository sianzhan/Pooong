using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Pooong
{
    partial struct HeartSpawningSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PooongConfig>();
            state.RequireForUpdate<PooongStageConfig>();
            state.RequireForUpdate<GameState>();
            state.RequireForUpdate<HeartSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawner = SystemAPI.GetSingleton<HeartSpawner>();
            var stageConfigs = SystemAPI.GetSingletonBuffer<PooongStageConfig>();
            var gameState = SystemAPI.GetSingletonRW<GameState>();
            var currentStageConfig = stageConfigs[gameState.ValueRO.CurrentStage];

            if (ShouldSpawnHeart(ref state, gameState, currentStageConfig))
            {
                SpawnHeart(ref state, gameState, currentStageConfig, spawner);
            }
        }

        [BurstCompile]
        private int GetFreeHeartCount(ref SystemState state)
        {
            var queryHeartNotInCart = SystemAPI.QueryBuilder().WithAll<Heart>().WithNone<CartItem>().Build();

            return queryHeartNotInCart.CalculateEntityCount();
        }

        [BurstCompile]
        private bool ShouldSpawnHeart(ref SystemState state, RefRW<GameState> game, in PooongStageConfig currentStageConfig)
        {
            var heartCount = GetFreeHeartCount(ref state);

            if (!game.ValueRO.Running) return false;
            if (heartCount >= currentStageConfig.MaxConcurrentHearts) return false;
            if (SystemAPI.Time.ElapsedTime < game.ValueRO.LastSpawnTime + currentStageConfig.SpawnInterval) return false;

            return true;
        }

        [BurstCompile]
        private void SpawnHeart(ref SystemState state, RefRW<GameState> game, in PooongStageConfig currentStageConfig, in HeartSpawner spawner)
        {
            var heart = state.EntityManager.Instantiate(spawner.HeartPrefab);
            var position = new float3(GetRandomXY(currentStageConfig.SpawnRadius), 0f);

            position.y += 2;

            state.EntityManager.SetComponentData(heart, LocalTransform.FromPosition(position));

            var velocityFactor = UnityEngine.Random.Range(currentStageConfig.InitialHeartVelocityMin, currentStageConfig.InitialHeartVelocityMax);
            var velocity = new float3(math.normalize(GetRandomXY()) * velocityFactor, 0f);

            state.EntityManager.SetComponentData(heart, new PhysicsVelocity
            {
                Linear = velocity
            });

            game.ValueRW.AmountSpawnedThisStage += 1;
            game.ValueRW.LastSpawnTime = SystemAPI.Time.ElapsedTime;
        }

        [BurstCompile]
        private readonly float2 GetRandomXY(float radius = 1)
        {
            return UnityEngine.Random.insideUnitCircle * radius;
        }
    }
}