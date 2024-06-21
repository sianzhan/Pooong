using UnityEngine;
using Unity.Entities;

namespace Pooong
{
    public struct PlayerInputData : IComponentData
    {
        public Vector2 MoveValue;
    }
}