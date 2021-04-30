using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarWalkMovement : AvatarMovementBase {
        [SerializeField]
        bool resetVelocity = true;
        [SerializeField, MyBox.ConditionalField(nameof(resetVelocity))]
        Vector2 initialVelocity = Vector2.zero;
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
            var targetVelocity = avatar.movementInput * speed;
            targetVelocity.y = avatar.velocity.y;
            avatar.velocity = Vector2.SmoothDamp(avatar.velocity, targetVelocity, ref acceleration, duration);
        }
    }
}