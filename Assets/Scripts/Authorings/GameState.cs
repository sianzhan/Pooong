using Unity.Entities;
using Unity.Collections;

namespace Pooong
{
    public struct GameState : IComponentData
    {
        public int CurrentStage;
        public int AmountSpawnedThisStage;
        public int CartedHeartCount;
        public int TargetCartedHeartCount;
        public int BrokenHeartCount;
        public int TargetBrokenHeartCount;
        public double LastSpawnTime;
    }
}