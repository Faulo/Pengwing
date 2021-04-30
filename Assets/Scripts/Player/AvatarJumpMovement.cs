using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarJumpMovement : AvatarMovementBase {
        [SerializeField, Range(0, 100)]
        float initialSpeed = 10;
        [SerializeField, Range(0, 1)]
        float inputDeadZone = 0;
        [SerializeField]
        Vector2 defaultVelocity = Vector2.one;

        public override void EnterMovement(AvatarController avatar) {
            var targetVelocity = avatar.movementInput.magnitude > inputDeadZone
                ? avatar.movementInput.normalized
                : defaultVelocity;
            avatar.velocity += initialSpeed * targetVelocity;
        }
        public override void UpdateMovement(AvatarController avatar) {
        }
    }
}