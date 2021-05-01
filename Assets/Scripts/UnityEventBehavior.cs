using UnityEngine;
using UnityEngine.Events;

namespace Runtime {
    public class UnityEventBehavior : StateMachineBehaviour {
        [SerializeField]
        UnityEvent onStateEnter = default;
        [SerializeField]
        UnityEvent onStateExit = default;

        GameObject instance;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            onStateEnter.Invoke();
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            onStateExit.Invoke();
        }
    }
}