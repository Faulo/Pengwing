using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Level {
    public class VisionLightComponent : MonoBehaviour {
        [SerializeField, Expandable]
        UnityEngine.Rendering.Universal.Light2D attachedLight = default;

        IVisionComponent vision;

        protected void Awake() {
            OnValidate();
        }
        protected void OnValidate() {
            if (!attachedLight) {
                TryGetComponent(out attachedLight);
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
            if (path.Length == 0) {
                path = new[] { (Vector2)transform.position };
            }
#if UNITY_EDITOR
            if (attachedLight.shapePath == null || attachedLight.shapePath.Length != path.Length) {
                var lightObj = new UnityEditor.SerializedObject(attachedLight);
                lightObj.FindProperty("m_ShapePath").arraySize = path.Length;
                lightObj.ApplyModifiedProperties();
            }
#endif
            for (int i = 0; i < path.Length; i++) {
                attachedLight.shapePath[i] = path[i];
            }
        }
    }
}