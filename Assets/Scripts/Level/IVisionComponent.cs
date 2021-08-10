using System;
using UnityEngine;

namespace Runtime.Level {
    public interface IVisionComponent {
        public event Action<Vector2[]> onPathChanged;
    }
}