using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Pooong
{
    partial class MeerkatAnimationSystem : SystemBase
    {
        EntityQuery query;

        protected override void OnCreate()
        {
            RequireForUpdate<PlayerInputData>();
            query = new EntityQueryBuilder(Allocator.Temp).WithAll<MeerkatAnimation, SpriteRenderer>().Build(this);
        }

        protected override void OnUpdate()
        {
            var chunks = query.ToArchetypeChunkArray(Allocator.Temp);
            var playerInputData = SystemAPI.GetSingleton<PlayerInputData>();
            var entityType = GetEntityTypeHandle();

            foreach (var chunk in chunks)
            {
                var entities = chunk.GetNativeArray(entityType);

                for (var i = 0; i < chunk.Count; ++i)
                {
                    var entity = entities[i];
                    
                    var animation = EntityManager.GetComponentObject<MeerkatAnimation>(entity);
                    var renderer = EntityManager.GetComponentObject<SpriteRenderer>(entity);
                    var moving = playerInputData.MoveValue != Vector2.zero;

                    Animate(animation, renderer, moving, SystemAPI.Time.ElapsedTime);
                }
            }
        }

        private void Animate(MeerkatAnimation animation, SpriteRenderer renderer, bool moving, double currentTime)
        {
            if (currentTime - animation.LastAnimationTime < animation.Interval) return;

            if (!moving && animation.IndexMoveCurrent == 0) return;

            animation.IndexMoveCurrent = (animation.IndexMoveCurrent + 1) % animation.MoveAnims.Length;

            renderer.sprite = animation.MoveAnims[animation.IndexMoveCurrent];

            animation.LastAnimationTime = currentTime;
        }
    }
}