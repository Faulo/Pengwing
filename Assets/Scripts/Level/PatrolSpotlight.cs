using System;
using Runtime.Level.VisionCones;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime.Level {
    public class PatrolSpotlight : MonoBehaviour, IVisionComponent {
        public event Action<Vector2[]> onPathChanged;
        internal enum VisionConeAlgorithm {
            Null,
            ConstantRaycastCount,
            DynamicRaycastCount,
            WallTrackingRaycasts,
        }
        internal enum UpdateMethod {
            Update,
            FixedUpdate,
            LateUpdate,
        }
        [Header("Vision Cone algorithm")]
        [SerializeField]
        internal VisionConeAlgorithm algorithm = VisionConeAlgorithm.ConstantRaycastCount;
        [SerializeField]
        internal LayerMask rayLayers = default;
        [SerializeField, Range(-180, 180)]
        internal float startAngle = -60;
        [SerializeField, Range(-180, 180)]
        internal float stopAngle = 60;
        [SerializeField, Range(0, 1000)]
        internal float distance = 100;

        [Header("Vision Cone: Constant Raycasts")]
        [SerializeField, Range(0, 1000)]
        internal int rayCount = 100;

        [Header("Vision Cone: Dynamic Raycasts")]
        [SerializeField, Expandable]
        internal MeshCollider rayCollider = default;

        [SerializeField]
        internal UpdateMethod updateMethod = UpdateMethod.FixedUpdate;

        IVisionCone cone;
        Vector2[] path = Array.Empty<Vector2>();

        void CreateCone() {
            cone = algorithm switch {
                VisionConeAlgorithm.Null => new NullVisionCone(),
                VisionConeAlgorithm.ConstantRaycastCount => new ConstantRaycastVisionCone(rayCount),
                VisionConeAlgorithm.DynamicRaycastCount => new DynamicRaycastVisionCone(rayCollider),
                VisionConeAlgorithm.WallTrackingRaycasts => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
            cone.Setup(transform, rayLayers, startAngle, stopAngle, distance);
        }
        void UpdateCone() {
            bool hasChanged = false;
            if (cone == null) {
                CreateCone();
                hasChanged = true;
            }

            if (path.Length != cone.vertexCount) {
                path = new Vector2[cone.vertexCount];
                hasChanged = true;
            }

            int i = 0;
            foreach (var point in cone.GetVertices()) {
                if (path[i] != point) {
                    path[i] = point;
                    hasChanged = true;
                }

                i++;
            }

            if (hasChanged) {
                onPathChanged?.Invoke(path);
            }
        }
        protected void Update() {
            if (updateMethod == UpdateMethod.Update) {
                UpdateCone();
            }
        }
        protected void FixedUpdate() {
            if (updateMethod == UpdateMethod.FixedUpdate) {
                UpdateCone();
            }
        }
        protected void LateUpdate() {
            if (updateMethod == UpdateMethod.LateUpdate) {
                UpdateCone();
            }
        }
        protected void OnDrawGizmos() {
            var startOffset = transform.rotation * Quaternion.Euler(0, 0, -startAngle) * new Vector3(distance, 0, 0);
            var stopOffset = transform.rotation * Quaternion.Euler(0, 0, -stopAngle) * new Vector3(distance, 0, 0);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                transform.position,
                transform.position + startOffset
            );
            Gizmos.DrawLine(
                transform.position + startOffset,
                transform.position + stopOffset
            );
            Gizmos.DrawLine(
                transform.position,
                transform.position + stopOffset
            );
        }
    }
}