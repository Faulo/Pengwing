using System;
using Runtime.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime {
    public class SpotlightVolumeController : MonoBehaviour {
        enum ObservationMode {
            IsSeen,
            IsDead
        }
        [SerializeField]
        Volume attachedVolume = default;
        [SerializeField]
        ObservationMode mode = default;

        [SerializeField, Range(0, 10)]
        float maximumDuration = 1;
        [SerializeField, Range(0, 10)]
        float maximumWeight = 1;

        float timer;

        protected void Awake() {
            OnValidate();
        }
        protected void OnValidate() {
            if (!attachedVolume) {
                TryGetComponent(out attachedVolume);
            }
        }
        protected void Update() {
            if (!AvatarController.instance) {
                return;
            }

            bool increase = mode switch {
                ObservationMode.IsSeen => AvatarController.instance.isSeen,
                ObservationMode.IsDead => !AvatarController.instance.isAlive,
                _ => throw new NotImplementedException(mode.ToString()),
            };
            if (increase) {
                timer += Time.deltaTime / maximumDuration;
            } else {
                timer -= Time.deltaTime / maximumDuration;
            }

            timer = Mathf.Clamp01(timer);
            attachedVolume.weight = maximumWeight * timer;
        }
    }
}