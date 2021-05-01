using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarJumpMovement : AvatarMovementBase {
        [SerializeField, Range(0, 100)]
        float initialSpeed = 10;
        [SerializeField]
        bool allowTurning = false;
        [SerializeField, MyBox.ConditionalField(nameof(allowTurning)), Range(0, 1)]
        float turningDeadZone = 0;
        [SerializeField]
        bool allowInput = false;
        [SerializeField, MyBox.ConditionalField(nameof(allowInput)), Range(0, 1)]
        float inputDeadZone = 0;
        [SerializeField]
        Vector2 defaultVelocity = Vector2.one;

        public override void EnterMovement(AvatarController avatar) {
            if (allowTurning) {
                if (Mathf.Abs(avatar.movementInput.x) > turningDeadZone) {
                    avatar.isFacingLeft = Mathf.Sign(avatar.movementInput.x) < 0;
                }
            }
            var targetVelocity = allowInput && avatar.movementInput.magnitude > inputDeadZone
                ? avatar.movementInput.normalized
                : new Vector2(defaultVelocity.x * avatar.facingMultiplier, defaultVelocity.y);
            avatar.velocity += initialSpeed * targetVelocity;
        }
        public override void UpdateMovement(AvatarController avatar) {
        }
    }
}