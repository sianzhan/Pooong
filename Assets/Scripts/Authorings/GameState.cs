using Unity.Entities;

namespace Pooong
{
    public struct GameState : IComponentData
    {
        public bool Running;
        public bool MissionAccomplished;
        public int CurrentStage;
        public int AmountSpawnedThisStage;
        public int CartedHeartCount;
        public int TargetCartedHeartCount;
        public int BrokenHeartCount;
        public int TargetBrokenHeartCount;
        public double LastSpawnTime;
    }
}