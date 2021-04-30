using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    public class AvatarInput : MonoBehaviour {
        [SerializeField]
        AvatarController attachedAvatar = default;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedAvatar) {
                TryGetComponent(out attachedAvatar);
            }
        }
        void Update() {
            attachedAvatar.intendsToJump = Gamepad.current.aButton.isPressed;
            attachedAvatar.movementInput = Gamepad.current.leftStick.ReadValue();
        }
    }
}