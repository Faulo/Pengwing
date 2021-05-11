using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Player {
    public class AvatarBehavior : StateMachineBehaviour {
        protected AvatarController avatar;
        bool isInitialized = false;
        protected virtual void OnStateInitialize(Animator animator) {
            animator.TryGetComponent(out avatar);
            Assert.IsTrue(avatar);
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (!isInitialized) {
                isInitialized = true;
                OnStateInitialize(animator);
            }
        }
    }
}