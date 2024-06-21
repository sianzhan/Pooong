using Unity.Entities;
using UnityEngine;

namespace Pooong
{
    public class FragileHeartPieceAuthoring : MonoBehaviour
    {
        public class Baker : Baker<FragileHeartPieceAuthoring>
        {
            public override void Bake(FragileHeartPieceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new HeartBreaker());
            }
        }
    }
}