using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Level.VisionCones {
    public class RaycastVisionCone : IVisionCone {
        readonly int rayCount;

        Transform origin;
        LayerMask layers;
        float startAngle;
        float stopAngle;
        float distance;

        public int vertexCount => rayCount + 1;

        public RaycastVisionCone(int rayCount) {
            this.rayCount = rayCount;
        }
        public void Setup(Transform origin, LayerMask layers, float startAngle, float stopAngle, float distance) {
            this.origin = origin;
            this.layers = layers;
            this.startAngle = startAngle;
            this.stopAngle = stopAngle;
            this.distance = distance;
        }
        public IEnumerable<Vector3> GetVertices() {
            yield return Vector3.zero;
            float delta = (stopAngle - startAngle) / rayCount;
            for (int i = 0; i < rayCount; i++) {
                var rotation = origin.rotation * Quaternion.Euler(0, 0, startAngle + (delta * i));
                var ray = new Ray(origin.position, rotation * Vector3.right);
                yield return Physics.Raycast(ray, out var hit, distance, layers)
                    ? hit.point - origin.position
                    : ray.direction * distance;
            }
        }
    }
}