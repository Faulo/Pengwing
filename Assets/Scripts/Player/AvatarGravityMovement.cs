using UnityEngine;

namespace Runtime.Player {
    [CreateAssetMenu]
    public class AvatarGravityMovement : AvatarMovementBase {
        [SerializeField, Range(-10, 10)]
        float multiplier = 1;

        public override void EnterMovement(AvatarController avatar) {
            avatar.velocity += multiplier * Time.deltaTime * Physics2D.gravity;
        }
        public override void UpdateMovement(AvatarController avatar) {
            avatar.velocity += multiplier * Time.deltaTime * Physics2D.gravity;
        }
    }
}