using TMPro;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Pooong
{
    public class UIAuthoring : MonoBehaviour
    {
        public TextMeshProUGUI CartedHeartCountText;
        public TextMeshProUGUI CartedHeartTargetText;
        public TextMeshProUGUI BrokenHeartCountText;
        public TextMeshProUGUI BrokenHeartTargetText;

        private void Update()
        {
            var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UIUpdateSystem>();

            CartedHeartCountText.SetText(handle.CartedHeartCount.ToString());
            CartedHeartTargetText.SetText(handle.CartedHeartTarget.ToString());
            BrokenHeartCountText.SetText(handle.BrokenHeartCount.ToString());
            BrokenHeartTargetText.SetText(handle.BrokenHeartTarget.ToString());
        }
    }
}