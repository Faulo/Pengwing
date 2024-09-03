using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player {
    public class AvatarInput : MonoBehaviour {
        [SerializeField]
        AvatarController attachedAvatar = default;
        [SerializeField]
        InputActionAsset avatarActionAsset = default;

        InputActionAsset avatarActionInstance;

        protected void Awake() {
            OnValidate();
        }
        protected void OnValidate() {
            if (!attachedAvatar) {
                TryGetComponent(out attachedAvatar);
            }
        }
        protected void OnEnable() {
            avatarActionInstance = Instantiate(avatarActionAsset);
            avatarActionInstance.FindActionMap("Avatar").actionTriggered += HandleAction;
            avatarActionInstance.Enable();
        }
        protected void OnDisable() {
            Destroy(avatarActionInstance);
        }
        void HandleAction(InputAction.CallbackContext context) {
            switch (context.action.name) {
                case "Move":
                    attachedAvatar.movementInput = context.ReadValue<Vector2>();
                    break;
                case "Jump":
                    if (context.started) {
                        attachedAvatar.Jump();
                    }

                    break;
            }
        }
    }
}