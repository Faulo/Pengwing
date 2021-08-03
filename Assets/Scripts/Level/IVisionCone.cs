using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Level {
    public interface IVisionCone {
        void Setup(Transform origin, LayerMask layers, float startAngle, float stopAngle, float distance);
        int vertexCount { get; }
        IEnumerable<Vector3> GetVertices();
    }
}