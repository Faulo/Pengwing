using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Level.VisionCones {
    public class ConstantRaycastVisionCone : IVisionCone {
        readonly int rayCount;

        Transform origin;
        LayerMask layers;
        float startAngle;
        float stopAngle;
        float distance;

        public int vertexCount => rayCount + 1;

        public ConstantRaycastVisionCone(int rayCount) {
            this.rayCount = rayCount;
            Assert.IsTrue(rayCount >= 2, "cone needs at least 2 rays");
        }
        public void Setup(Transform origin, LayerMask layers, float startAngle, float stopAngle, float distance) {
            this.origin = origin;
            this.layers = layers;
            this.startAngle = startAngle;
            this.stopAngle = stopAngle;
            this.distance = distance;
            Assert.IsTrue(stopAngle >= startAngle, "stopAngle must be greater than startAngle");
        }
        public IEnumerable<Vector2> GetVertices() {
            yield return Vector2.zero;
            float delta = (stopAngle - startAngle) / (rayCount - 1);
            for (int i = 0; i < rayCount; i++) {
                var rotation = Quaternion.Euler(0, 0, -(startAngle + (delta * i)));
                var ray = new Ray(origin.position, origin.TransformDirection(rotation * Vector3.right));
                var point = Physics.Raycast(ray, out var hit, distance, layers)
                    ? hit.point
                    : ray.GetPoint(distance);
                yield return origin.InverseTransformPoint(point);
            }
        }
    }
}