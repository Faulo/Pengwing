using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runtime.Level {
    public class VisionLightComponent : MonoBehaviour {
        [SerializeField, Expandable]
        Light2D attachedLight = default;

        IVisionComponent vision;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedLight) {
                TryGetComponent(out attachedLight);
            }
        }
        void OnEnable() {
            vision = GetComponentInParent<IVisionComponent>();
            Assert.IsNotNull(vision);
            vision.onPathChanged += HandlePathChanged;
        }
        void OnDisable() {
            vision.onPathChanged -= HandlePathChanged;
        }
        void HandlePathChanged(Vector2[] path) {
            if (path.Length == 0) {
                path = new[] { (Vector2)transform.position };
            }
#if UNITY_EDITOR
            if (attachedLight.shapePath.Length != path.Length) {
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