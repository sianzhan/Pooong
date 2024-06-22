using TMPro;
using Unity.Entities;
using UnityEngine;

namespace Pooong
{
    public class UIAuthoring : MonoBehaviour
    {
        public TextMeshProUGUI CartedHeartCountText;
        public TextMeshProUGUI CartedHeartTargetText;
        public TextMeshProUGUI BrokenHeartCountText;
        public TextMeshProUGUI BrokenHeartTargetText;

        public GameObject InGameDisplay;
        public GameObject CongratulationText;
        public GameObject SeeYouNextTImeText;

        private void Start()
        {
            var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UIUpdateSystem>();
            handle.GameEnded = OnGameEnded;

            InGameDisplay.SetActive(true);

            CongratulationText.SetActive(false);
            SeeYouNextTImeText.SetActive(false);
        }

        private void Update()
        {
            var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UIUpdateSystem>();

            var ratio = (float)handle.BrokenHeartCount / handle.BrokenHeartTarget * 0.8f + 0.2f;
            BrokenHeartCountText.color = Color.HSVToRGB(0.97f, 1f, ratio);

            CartedHeartCountText.SetText(handle.CartedHeartCount.ToString());
            CartedHeartTargetText.SetText(handle.CartedHeartTarget.ToString());
            BrokenHeartCountText.SetText(handle.BrokenHeartCount.ToString());
            BrokenHeartTargetText.SetText(handle.BrokenHeartTarget.ToString());
        }

        private void OnGameEnded(bool shouldCongratz)
        {
            InGameDisplay.SetActive(false);

            if (shouldCongratz) CongratulationText.SetActive(true);
            else SeeYouNextTImeText.SetActive(true);
        }
    }
}