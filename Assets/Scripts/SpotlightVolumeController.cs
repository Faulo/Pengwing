using Runtime.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime {
    public class SpotlightVolumeController : MonoBehaviour {
        [SerializeField]
        Volume attachedVolume = default;

        AvatarController avatar;

        [SerializeField, Range(0, 10)]
        float maximumDuration = 1;
        [SerializeField, Range(0, 10)]
        float maximumWeight = 1;

        float timer;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedVolume) {
                TryGetComponent(out attachedVolume);
            }
        }
        void Start() {
            avatar = FindObjectOfType<AvatarController>();
        }
        void Update() {
            if (avatar.canFly) {
                timer -= Time.deltaTime / maximumDuration;
            } else {
                timer += Time.deltaTime / maximumDuration;
            }
            timer = Mathf.Clamp01(timer);
            attachedVolume.weight = maximumWeight * timer;
        }
    }
}