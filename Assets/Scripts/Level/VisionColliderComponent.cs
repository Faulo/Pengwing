using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Level {
    public class VisionColliderComponent : MonoBehaviour {
        [SerializeField, Expandable]
        PolygonCollider2D attachedCollider = default;

        IVisionComponent vision;

        protected void Awake() {
            OnValidate();
        }
        protected void OnValidate() {
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
        }
        protected void OnEnable() {
            vision = GetComponentInParent<IVisionComponent>();
            Assert.IsNotNull(vision);
            vision.onPathChanged += HandlePathChanged;
        }
        protected void OnDisable() {
            vision.onPathChanged -= HandlePathChanged;
        }
        void HandlePathChanged(Vector2[] path) {
            attachedCollider.pathCount = 1;
            attachedCollider.SetPath(0, path);
        }

        [Header("Debug")]
        [SerializeField]
        bool drawGizmos = false;
        protected void OnDrawGizmos() {
            if (drawGizmos) {
                Gizmos.color = Color.cyan;
                foreach (var point in attachedCollider.GetPath(0)) {
                    Gizmos.DrawLine(transform.position, transform.position + point.SwizzleXY());
                }
            }
        }
    }
}