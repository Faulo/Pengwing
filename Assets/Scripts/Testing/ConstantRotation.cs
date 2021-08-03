using UnityEngine;

namespace Runtime.Testing {
    public class ConstantRotation : MonoBehaviour {
        [SerializeField]
        Quaternion rotation = Quaternion.identity;
        void FixedUpdate() {
            transform.rotation *= rotation;
        }
    }
}