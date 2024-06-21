using UnityEngine;
using Unity.Entities;

namespace Pooong
{
    public class BallAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BallAuthoring>
        {
            public override void Bake(BallAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Ball>(entity);
            }
        }
    }

    public struct Ball : IComponentData { }
}