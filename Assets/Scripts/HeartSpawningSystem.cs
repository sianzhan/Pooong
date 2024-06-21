using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

namespace Pooong
{
    partial struct HeartSpawningSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (GetHeartCount(ref state) < SystemAPI.GetSingleton<PooongConfig>().MaxConcurrentHeartAmount)
            {
                SpawnHeart(ref state);
            }
        }

        [BurstCompile]
        private int GetHeartCount(ref SystemState state)
        {
            var query = SystemAPI.QueryBuilder().WithAll<Heart>().Build();

            return query.CalculateEntityCount();
        }

        [BurstCompile]
        private void SpawnHeart(ref SystemState state)
        {
            var spawner = SystemAPI.GetSingleton<HeartSpawner>();

            var heart = state.EntityManager.Instantiate(spawner.HeartPrefab);
            var position = new float3(GetRandomXY(spawner.SpawnRadius), 0f);

            state.EntityManager.SetComponentData(heart, LocalTransform.FromPosition(position));

            var velocityFactor = UnityEngine.Random.Range(spawner.InitialVelocityMin, spawner.InitialVelocityMax);
            var velocity = new float3(math.normalize(GetRandomXY()) * velocityFactor, 0f);

            state.EntityManager.SetComponentData(heart, new PhysicsVelocity
            {
                Linear = velocity
            });
        }

        [BurstCompile]
        private readonly float2 GetRandomXY(float radius = 1)
        {
            return UnityEngine.Random.insideUnitCircle * radius;
        }
    }
}