using UnityEngine;

namespace Runtime.Player {
    public class PlayerHUD : MonoBehaviour {
        [SerializeField]
        GameObject canFlyObject = default;
        [SerializeField]
        GameObject cannotFlyObject = default;

        AvatarController attachedAvatar;

        protected void Start() {
            attachedAvatar = FindObjectOfType<AvatarController>();
        }
        protected void Update() {
            canFlyObject.SetActive(attachedAvatar.canFly);
            cannotFlyObject.SetActive(!attachedAvatar.canFly);
        }
    }
}