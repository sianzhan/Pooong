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
        InputAction rushAction;

        [BurstCompile]
        protected override void OnCreate()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            rushAction = InputSystem.actions.FindAction("Rush");

            EntityManager.AddComponent<PlayerInputData>(SystemHandle);
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var playerInputData = SystemAPI.GetSingletonRW<PlayerInputData>();

            playerInputData.ValueRW.MoveValue = moveAction.ReadValue<Vector2>();
            playerInputData.ValueRW.RushModeToggled = rushAction.IsPressed();

        }
    }
}