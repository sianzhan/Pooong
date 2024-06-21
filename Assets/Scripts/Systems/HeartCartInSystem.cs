using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace Pooong
{
    partial struct HeartCartInSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PooongConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            var heartLookup = SystemAPI.GetComponentLookup<Heart>();
            var cartLookup = SystemAPI.GetComponentLookup<Cart>();
            var massLookup = SystemAPI.GetComponentLookup<PhysicsMass>();
            var gravityLookup = SystemAPI.GetComponentLookup<PhysicsGravityFactor>();
            var config = SystemAPI.GetSingleton<PooongConfig>();

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            state.Dependency = new HeartCartInTriggerEventJob
            {
                HeartLookup = heartLookup,
                CartLookup = cartLookup,
                MassLookup = massLookup,
                GravityLookup = gravityLookup,
                Config = config,
                Ecb = ecb
            }.Schedule(simulation, state.Dependency);
        }

        [BurstCompile]
        public partial struct HeartCartInTriggerEventJob : ITriggerEventsJob
        {
            public PooongConfig Config;
            public ComponentLookup<Heart> HeartLookup;
            public ComponentLookup<Cart> CartLookup;
            public ComponentLookup<PhysicsMass> MassLookup;
            public ComponentLookup<PhysicsGravityFactor> GravityLookup;
            public EntityCommandBuffer Ecb;

            [BurstCompile]
            public void Execute(TriggerEvent trigger)
            {
                Entity heartEntity;

                if (HeartLookup.HasComponent(trigger.EntityA) && CartLookup.HasComponent(trigger.EntityB))
                {
                    heartEntity = trigger.EntityA;
                }
                else if (HeartLookup.HasComponent(trigger.EntityB) && CartLookup.HasComponent(trigger.EntityA))
                {
                    heartEntity = trigger.EntityB;
                }
                else return;

                var heart = HeartLookup.GetRefRW(heartEntity);
                var gravity = GravityLookup.GetRefRW(heartEntity);

                heart.ValueRW.Breakable = false;
                gravity.ValueRW.Value = Config.GravityFactorInCart;

                Ecb.AddComponent(heartEntity, new CartItem());
            }
        }
    }
}