using Runtime.Player;
using UnityEngine;

namespace Runtime.Level {
    public class CrocDetector : MonoBehaviour {
        [SerializeField]
        LayerMask solidLayer = default;
        [SerializeField]
        LayerMask swampLayer = default;
        [SerializeField, Range(0, 1)]
        float swampRadius = 1;
        [SerializeField]
        GameObject crocPrefab = default;
        [SerializeField, Range(0, 10)]
        float crocDuration = 1;

        GameObject crocInstance;

        protected void FixedUpdate() {
            if (crocInstance) {
                return;
            }

            if (!AvatarController.instance || !AvatarController.instance.isAlive) {
                return;
            }

            if (Physics.CheckSphere(transform.position, swampRadius, solidLayer, QueryTriggerInteraction.Collide)) {
                return;
            }

            if (Physics2D.OverlapCircle(transform.position, swampRadius, swampLayer)) {
                crocInstance = Instantiate(crocPrefab, transform.position, Quaternion.identity);
                Destroy(crocInstance, crocDuration);
            }
        }
    }
}