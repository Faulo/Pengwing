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

        public override void EnterMovement(AvatarController avatar) {
            if (resetVelocity) {
                avatar.velocity = initialVelocity;
            }
        }
        public override void UpdateMovement(AvatarController avatar) {
            avatar.velocity.x = (avatar.movementInput * speed).x;
        }
    }
}