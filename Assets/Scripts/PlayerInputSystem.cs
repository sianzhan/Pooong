using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pooong
{
    partial class PlayerInputSystem : SystemBase
    {
        InputAction moveAction;

        [BurstCompile]
        protected override void OnCreate()
        {
            moveAction = InputSystem.actions.FindAction("Move");

            EntityManager.AddComponent<PlayerInputData>(SystemHandle);
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var playerInputData = SystemAPI.GetComponentRW<PlayerInputData>(SystemHandle);

            playerInputData.ValueRW.MoveValue = moveAction.ReadValue<Vector2>();
        }
    }
}