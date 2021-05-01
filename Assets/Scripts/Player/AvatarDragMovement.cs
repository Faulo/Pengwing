using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarDragMovement : AvatarMovementBase {
        [SerializeField]
        Vector2 targetVelocity = Vector2.zero;
        [SerializeField, Range(0, 100)]
        float duration = 0;

        Vector2 acceleration;

        public override void EnterMovement(AvatarController avatar) {
            acceleration = Vector2.zero;
        }
        public override void UpdateMovement(AvatarController avatar) {
            avatar.velocity = Vector2.SmoothDamp(avatar.velocity, new Vector2(avatar.facingMultiplier * targetVelocity.x, targetVelocity.y), ref acceleration, duration);
        }
    }
}