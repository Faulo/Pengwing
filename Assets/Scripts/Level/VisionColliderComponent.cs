using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Level {
    public class VisionColliderComponent : MonoBehaviour {
        [SerializeField, Expandable]
        PolygonCollider2D attachedCollider = default;

        IVisionComponent vision;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
        }
        void OnEnable() {
            vision = GetComponentInParent<IVisionComponent>();
            Assert.IsNotNull(vision);
            vision.onPathChanged += HandlePathChanged;
        }
        void OnDisable() {
            vision.onPathChanged -= HandlePathChanged;
        }
        void HandlePathChanged(Vector2[] path) {
            attachedCollider.pathCount = 1;
            attachedCollider.SetPath(0, path);
        }

        [Header("Debug")]
        [SerializeField]
        bool drawGizmos = false;
        void OnDrawGizmos() {
            if (drawGizmos) {
                Gizmos.color = Color.cyan;
                foreach (var point in attachedCollider.GetPath(0)) {
                    Gizmos.DrawLine(transform.position, transform.position + point.SwizzleXY());
                }
            }
        }
    }
}