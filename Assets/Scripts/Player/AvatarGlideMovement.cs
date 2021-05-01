using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarGlideMovement : AvatarMovementBase {
        [SerializeField]
        bool resetVelocity = true;
        [SerializeField, MyBox.ConditionalField(nameof(resetVelocity))]
        Vector2 initialVelocity = Vector2.zero;
        [SerializeField, Range(0, 1)]
        float inputDeadZone = 0;
        [SerializeField, Range(0, 100)]
        float speed = 10;
        [SerializeField, Range(0, 100)]
        float duration = 0;

        Vector2 acceleration;

        public override void EnterMovement(AvatarController avatar) {
            if (resetVelocity) {
                avatar.velocity = initialVelocity;
            }
        }
        public override void UpdateMovement(AvatarController avatar) {
            if (Mathf.Abs(avatar.movementInput.x) > inputDeadZone) {
                avatar.isFacingLeft = Mathf.Sign(avatar.movementInput.x) < 0;
            }
            var targetVelocity = avatar.movementInput * speed;
            avatar.velocity = Vector2.SmoothDamp(avatar.velocity, targetVelocity, ref acceleration, duration);
        }
    }
}