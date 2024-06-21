using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pooong
{
    [BurstCompile]
    partial class PlayerInputSystem : SystemBase
    {
        InputAction moveAction;
        InputAction slowAction;

        [BurstCompile]
        protected override void OnCreate()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            slowAction = InputSystem.actions.FindAction("Slow");

            EntityManager.AddComponent<PlayerInputData>(SystemHandle);
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var playerInputData = SystemAPI.GetSingletonRW<PlayerInputData>();

            playerInputData.ValueRW.MoveValue = moveAction.ReadValue<Vector2>();
            playerInputData.ValueRW.SlowModeToggled = slowAction.IsPressed();

        }
    }
}