using Runtime.Level.VisionCones;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runtime.Level {
    public class PatrolSpotlight : MonoBehaviour {
        enum VisionConeAlgorithm {
            Raycasts,
            WallTracking,
        }
        [Header("MonoBehaviour setup")]
        [SerializeField]
        Light2D attachedLight = default;
        [SerializeField]
        PolygonCollider2D attachedPolygon = default;

        [Header("Vision Cone algorithm")]
        [SerializeField]
        VisionConeAlgorithm algorithm = VisionConeAlgorithm.Raycasts;
        [SerializeField]
        LayerMask rayLayers = default;
        [SerializeField, Range(-180, 180)]
        float startAngle = -45;
        [SerializeField, Range(-180, 180)]
        float stopAngle = 45;
        [SerializeField, Range(0, 1000)]
        float distance = 100;

        [Header("Vision Cone: Raycasts")]
        [SerializeField, Range(0, 1000)]
        int rayCount = 100;

        IVisionCone cone {
            get {
                if (m_cone == null) {
                    m_cone = algorithm switch {
                        VisionConeAlgorithm.Raycasts => new RaycastVisionCone(rayCount),
                        VisionConeAlgorithm.WallTracking => throw new System.NotImplementedException(),
                        _ => throw new System.NotImplementedException(),
                    };
                    m_cone.Setup(transform, rayLayers, startAngle, stopAngle, distance);
                }
                return m_cone;
            }
        }
        IVisionCone m_cone;

        Vector3 position => transform.position;
        Vector2[] path;

        void Awake() {
            m_cone = null;
            path = new Vector2[cone.vertexCount];
            OnValidate();
        }
        void OnValidate() {
            if (!attachedLight) {
                TryGetComponent(out attachedLight);
            }
            if (!attachedPolygon) {
                TryGetComponent(out attachedPolygon);
            }
#if UNITY_EDITOR
            var lightObj = new UnityEditor.SerializedObject(attachedLight);
            lightObj.FindProperty("m_ShapePath").arraySize = cone.vertexCount;
            lightObj.ApplyModifiedProperties();
            path = new Vector2[cone.vertexCount];
            FixedUpdate();
#endif
        }
        void FixedUpdate() {
            int i = 0;
            foreach (var point in cone.GetVertices()) {
                attachedLight.shapePath[i] = path[i] = Quaternion.Inverse(transform.rotation) * point;
                i++;
            }
            attachedPolygon.pathCount = 1;
            attachedPolygon.SetPath(0, path);
        }

        [Header("Debug")]
        [SerializeField]
        bool drawGizmos = false;
        void OnDrawGizmos() {
            if (drawGizmos) {
                Gizmos.color = Color.cyan;
                foreach (var point in attachedPolygon.GetPath(0)) {
                    Gizmos.DrawLine(position, position + (transform.rotation * point.SwizzleXY()));
                }
            }
        }
    }
}