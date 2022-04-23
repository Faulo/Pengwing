using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Level.VisionCones {
    public class NullVisionCone : IVisionCone {
        public int vertexCount => 0;
        public IEnumerable<Vector2> GetVertices() => Array.Empty<Vector2>();
        public void Setup(Transform origin, LayerMask layers, float startAngle, float stopAngle, float distance) {
        }
    }
}