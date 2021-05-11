using UnityEngine;

namespace Runtime {
    public class SpawnObjectBehavior : StateMachineBehaviour {
        [SerializeField]
        GameObject prefab = default;
        [SerializeField]
        bool destroyOnExit = true;

        GameObject instance;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            instance = Instantiate(prefab, animator.transform);
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (destroyOnExit && instance) {               
                Destroy(instance);
            }
        }
    }
}