using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime.Player {
    public class AvatarMovementBehavior : AvatarBehavior {
        [SerializeField, Expandable]
        AvatarMovementBase movement = default;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            movement.EnterMovement(avatar);
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            movement.UpdateMovement(avatar);
        }
    }
}