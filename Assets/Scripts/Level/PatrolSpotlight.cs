using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runtime.Level {
    public class PatrolSpotlight : MonoBehaviour {
        [SerializeField]
        Light2D attachedLight = default;
        [SerializeField]
        PolygonCollider2D attachedPolygon = default;
        [SerializeField]
        LayerMask rayLayers = default;
        [SerializeField, Range(-90, 90)]
        float startAngle = -45;
        [SerializeField, Range(-90, 90)]
        float stopAngle = 45;
        [SerializeField, Range(0, 1000)]
        int rayCount = 100;
        [SerializeField, Range(0, 1000)]
        float distance = 100;

        Vector3 position => transform.position;
        Vector2[] path;
        int pathLength => rayCount + 1;

        IEnumerable<Vector3> allPoints {
            get {
                yield return Vector3.zero;
                float delta = (stopAngle - startAngle) / rayCount;
                for (int i = 0; i < rayCount; i++) {
                    var rotation = transform.rotation * Quaternion.Euler(0, 0, startAngle + (delta * i));
                    var ray = new Ray(position, rotation * Vector3.right);
                    yield return Physics.Raycast(ray, out var hit, distance, rayLayers)
                        ? hit.point - position
                        : ray.direction * distance;
                }
            }
        }

        void Awake() {
            path = new Vector2[pathLength];
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
            lightObj.FindProperty("m_ShapePath").arraySize = pathLength;
            lightObj.ApplyModifiedProperties();
            path = new Vector2[pathLength];
            FixedUpdate();
#endif
        }
        void FixedUpdate() {
            int i = 0;
            foreach (var point in allPoints) {
                attachedLight.shapePath[i] = path[i] = transform.rotation * point;
                i++;
            }
            attachedPolygon.pathCount = 1;
            attachedPolygon.SetPath(0, path);
        }
        void OnDrawGizmos() {
            /*
            Gizmos.color = Color.cyan;
            foreach (var point in allPoints) {
                Gizmos.DrawLine(position, position + point);
            }
            //*/
        }
    }
}