using System.Diagnostics;
using Pooong;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Pooong
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    partial struct HeartBreakingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            var heartLookup = SystemAPI.GetComponentLookup<Heart>();
            var heartBreakerLookup = SystemAPI.GetComponentLookup<HeartBreaker>();
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            var linkedLookup = SystemAPI.GetBufferLookup<LinkedEntityGroup>();

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            state.Dependency = new HeartCollisionEventJob
            {
                HeartLookup = heartLookup,
                HeartBreakerLookup = heartBreakerLookup,
                TransformLookup = transformLookup,
                LinkedLookup = linkedLookup,
                Ecb = ecb
            }.Schedule(simulation, state.Dependency);
        }

        [BurstCompile]
        public partial struct HeartCollisionEventJob : ICollisionEventsJob
        {
            public ComponentLookup<Heart> HeartLookup;
            public ComponentLookup<HeartBreaker> HeartBreakerLookup;
            public ComponentLookup<LocalTransform> TransformLookup;
            public BufferLookup<LinkedEntityGroup> LinkedLookup;
            public EntityCommandBuffer Ecb;

            [BurstCompile]
            public void Execute(CollisionEvent collisionEvent)
            {
                Entity heartEntity;

                if (HeartLookup.HasComponent(collisionEvent.EntityA) && HeartBreakerLookup.HasComponent(collisionEvent.EntityB))
                {
                    heartEntity = collisionEvent.EntityA;
                }
                else if (HeartLookup.HasComponent(collisionEvent.EntityB) && HeartBreakerLookup.HasComponent(collisionEvent.EntityA))
                {
                    heartEntity = collisionEvent.EntityB;
                }
                else return;

                var heart = HeartLookup.GetRefRO(heartEntity);
                var transform = TransformLookup.GetRefRO(heartEntity);

                LinkedLookup.TryGetBuffer(heart.ValueRO.FragileHeartPrefab, out DynamicBuffer<LinkedEntityGroup> linkedEntityGroup);

                foreach (var child in linkedEntityGroup)
                {
                    Ecb.SetComponent(child.Value, LocalTransform.FromPosition(transform.ValueRO.Position));
                }

                var entity = Ecb.Instantiate(heart.ValueRO.FragileHeartPrefab);

                Ecb.DestroyEntity(heartEntity);
            }
        }
    }
}