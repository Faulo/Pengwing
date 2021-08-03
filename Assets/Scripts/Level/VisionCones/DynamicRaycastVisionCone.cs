using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime.Level.VisionCones {
    public class DynamicRaycastVisionCone : IVisionCone {
        readonly MeshCollider meshCollider;

        Transform origin;
        LayerMask layers;
        float startAngle;
        float stopAngle;
        float distance;

        Vector3 lastCheckedPosition;
        readonly List<Vector3> vertices = new List<Vector3>();
        public int vertexCount {
            get {
                UpdateVertices();
                return vertices.Count;
            }
        }

        public DynamicRaycastVisionCone(MeshCollider meshCollider) {
            this.meshCollider = meshCollider;
        }
        public void Setup(Transform origin, LayerMask layers, float startAngle, float stopAngle, float distance) {
            this.origin = origin;
            this.layers = layers;
            this.startAngle = startAngle;
            this.stopAngle = stopAngle;
            this.distance = distance;
        }
        public IEnumerable<Vector3> GetVertices() {
            UpdateVertices();
            return vertices;
        }
        void UpdateVertices() {
            if (lastCheckedPosition != origin.position && meshCollider.sharedMesh) {
                lastCheckedPosition = origin.position;
                var potentialVertices = new SortedList<float, Vector3>();
                foreach (var vertex in meshCollider.sharedMesh.vertices) {
                    float angle = Vector2.SignedAngle(lastCheckedPosition, vertex);
                    if (!potentialVertices.ContainsKey(angle)) {
                        if (Vector3.Distance(vertex, lastCheckedPosition) <= distance) {
                            if (angle >= startAngle && angle <= stopAngle) {
                                potentialVertices.Add(angle, vertex);
                            }
                        }
                    }
                }
                vertices.Clear();
                foreach ((float angle, var vertex) in potentialVertices) {
                    var position = Physics.Linecast(origin.position, vertex, out var hit, layers)
                        ? hit.point - origin.position
                        : Quaternion.Euler(0, 0, angle) * new Vector3(0, 0, distance);
                    vertices.Add(position);
                }
            }
        }
    }
}