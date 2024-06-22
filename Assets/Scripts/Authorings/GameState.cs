using Unity.Entities;

namespace Pooong
{
    public struct GameState : IComponentData
    {
        public int CurrentStage;
        public int AmountSpawnedThisStage;
        public double LastSpawnTime;
    }
}