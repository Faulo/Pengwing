using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Level.VisionCones {
    public class DynamicRaycastVisionCone : IVisionCone {
        readonly MeshCollider meshCollider;

        Transform origin;
        LayerMask layers;
        float startAngle;
        float stopAngle;
        float distance;

        Vector3 lastCheckedPosition;
        Quaternion lastCheckedRotation;
        readonly List<Vector2> vertices = new List<Vector2>();
        public int vertexCount {
            get {
                UpdateVertices();
                return vertices.Count;
            }
        }

        public DynamicRaycastVisionCone(MeshCollider meshCollider) {
            this.meshCollider = meshCollider;
            Assert.IsTrue(meshCollider);
            Assert.IsTrue(meshCollider.sharedMesh);
            Assert.IsTrue(meshCollider.sharedMesh.vertexCount > 1);
        }
        public void Setup(Transform origin, LayerMask layers, float startAngle, float stopAngle, float distance) {
            this.origin = origin;
            this.layers = layers;
            this.startAngle = startAngle;
            this.stopAngle = stopAngle;
            this.distance = distance;
        }
        public IEnumerable<Vector2> GetVertices() {
            UpdateVertices();
            return vertices;
        }
        bool NeedsUpdating() {
            if (lastCheckedPosition != origin.position || lastCheckedRotation != origin.rotation) {
                lastCheckedPosition = origin.position;
                lastCheckedRotation = origin.rotation;
                return true;
            }
            return false;
        }
        void UpdateVertices() {
            if (NeedsUpdating()) {
                var potentialVertices = new SortedList<float, Vector2>();
                var startOffset = origin.rotation * Quaternion.Euler(0, 0, -startAngle) * new Vector3(distance, 0, 0);
                var stopOffset = origin.rotation * Quaternion.Euler(0, 0, -stopAngle) * new Vector3(distance, 0, 0);

                potentialVertices.Add(Mathf.RoundToInt(startAngle), lastCheckedPosition + startOffset);
                potentialVertices.Add(Mathf.RoundToInt(stopAngle), lastCheckedPosition + stopOffset);
                foreach (var vertex in meshCollider.sharedMesh.vertices) {
                    float angle = Vector2.SignedAngle(vertex - lastCheckedPosition, origin.right);
                    if (!potentialVertices.ContainsKey(angle)) {
                        if (angle >= startAngle && angle <= stopAngle) {
                            potentialVertices.Add(angle, vertex);
                        }
                    }
                }
                vertices.Clear();
                vertices.Add(Vector2.zero);
                foreach ((_, var vertex) in potentialVertices) {
                    var direction = vertex.SwizzleXY() - lastCheckedPosition;
                    var ray = new Ray(lastCheckedPosition, direction);
                    if (Physics.Raycast(ray, out var hit, distance, layers)) {
                        vertices.Add(origin.InverseTransformPoint(hit.point));
                        if (hit.distance > Vector2.Distance(origin.position, vertex)) {
                            vertices.Add(origin.InverseTransformPoint(vertex));
                        }
                    } else {
                        vertices.Add(origin.InverseTransformPoint(ray.GetPoint(distance)));
                        if (distance > Vector2.Distance(origin.position, vertex)) {
                            vertices.Add(origin.InverseTransformPoint(vertex));
                        }
                    }
                }
            }
        }
    }
}