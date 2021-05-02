using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarAtmosphereMovement : AvatarMovementBase {
        [SerializeField]
        AnimationCurve downDragOverY = AnimationCurve.Linear(1, 1, 100, 10);
        [SerializeField, Range(0, 100)]
        float duration = 0;

        Vector2 acceleration;

        public override void EnterMovement(AvatarController avatar) {
            acceleration = Vector2.zero;
        }
        public override void UpdateMovement(AvatarController avatar) {
            var targetVelocity = new Vector2(avatar.velocity.x, avatar.velocity.y - downDragOverY.Evaluate(avatar.transform.position.y));
            avatar.velocity = Vector2.SmoothDamp(avatar.velocity, targetVelocity, ref acceleration, duration);
        }
    }
}