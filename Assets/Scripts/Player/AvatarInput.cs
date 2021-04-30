using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player {
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
            if (Gamepad.current.aButton.wasPressedThisFrame) {
                attachedAvatar.Jump();
            }
            attachedAvatar.movementInput = Gamepad.current.leftStick.ReadValue();
        }
    }
}