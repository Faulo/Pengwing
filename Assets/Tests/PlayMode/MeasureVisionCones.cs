using System;
using System.Collections;
using NUnit.Framework;
using Runtime.Level;
using Runtime.Level.VisionCones;
using Runtime.Testing;
using Slothsoft.UnityExtensions;
using Unity.PerformanceTesting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.TestTools;

namespace PlayMode {
    [TestFixture]
    public class MeasureVisionCones {
        [Flags]
        public enum AdditionalComponents {
            Nothing = 0,
            Collider = 1 << 0,
            Light = 1 << 1,
            ColliderAndLight = Collider | Light,
        }
        const int WARMUP_COUNT = 50;
        const int MEASUREMENT_COUNT = 300;
        const string LEVEL_PREFAB = "Assets/Prefabs/Level_MeasureCone.prefab";

        public static readonly Type[] testCones = new[] {
            typeof(NullVisionCone),
            typeof(ConstantRaycastVisionCone),
            typeof(DynamicRaycastVisionCone),
        };

        public static readonly AdditionalComponents[] testComponents = new[] {
            AdditionalComponents.Collider,
            AdditionalComponents.Light,
            AdditionalComponents.ColliderAndLight,
        };
        public static readonly int[] raycastCounts = new[] { 2, 256, 1024 };

        GameObject cameraObj;
        Camera camera;

        GameObject levelObj;
        MeshCollider levelCollider;

        GameObject spotlightObj;
        PatrolSpotlight spotlight;

        [SetUp]
        public void SetUp() {
            cameraObj = new GameObject(nameof(Camera));
            cameraObj.transform.position = new Vector3(0, 0, -50);
            camera = cameraObj.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 20;
            camera.clearFlags = CameraClearFlags.Color;

            levelObj = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(LEVEL_PREFAB));
            levelCollider = levelObj.GetComponentsInChildren<MeshCollider>()[1];

            spotlightObj = new GameObject(nameof(MeasureVisionCones));
            var constantRotation = spotlightObj.AddComponent<ConstantRotation>();
            constantRotation.rotation = Quaternion.Euler(0, 0, 10);
            QualitySettings.vSyncCount = 0;
        }
        [TearDown]
        public void TearDown() {
            UnityEngine.Object.Destroy(cameraObj);
            UnityEngine.Object.Destroy(levelObj);
            UnityEngine.Object.Destroy(spotlightObj);
        }
        void AddAdditionalComponents(AdditionalComponents components) {
            if ((components & AdditionalComponents.Collider) != 0) {
                spotlightObj.AddComponent<PolygonCollider2D>();
                spotlightObj.AddComponent<VisionColliderComponent>();
            }
            if ((components & AdditionalComponents.Light) != 0) {
                spotlightObj.AddComponent<Light2D>();
                spotlightObj.AddComponent<VisionLightComponent>();
            }
        }
        IEnumerator MeasureTime() {
            var deltaTime = new SampleGroup("Time.deltaTime", SampleUnit.Millisecond);
            yield return Measure
                .Frames()
                .WarmupCount(WARMUP_COUNT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .SampleGroup(deltaTime)
                .Run();
        }
        [UnityTest, Performance]
        public IEnumerator MeasureNullCone(
            [ValueSource(nameof(testComponents))] AdditionalComponents components) {
            yield return Wait.forFixedUpdate;

            spotlight = spotlightObj.AddComponent<PatrolSpotlight>();
            spotlight.rayLayers = 1 << LayerMask.NameToLayer("Foreground");
            spotlight.algorithm = PatrolSpotlight.VisionConeAlgorithm.Null;

            AddAdditionalComponents(components);

            yield return MeasureTime();
        }
        [UnityTest, Performance]
        public IEnumerator MeasureConstantRaycastCone(
            [ValueSource(nameof(testComponents))] AdditionalComponents components,
            [ValueSource(nameof(raycastCounts))] int raycastCount) {
            yield return Wait.forFixedUpdate;

            spotlight = spotlightObj.AddComponent<PatrolSpotlight>();
            spotlight.rayLayers = 1 << LayerMask.NameToLayer("Foreground");
            spotlight.algorithm = PatrolSpotlight.VisionConeAlgorithm.ConstantRaycastCount;
            spotlight.rayCount = raycastCount;

            AddAdditionalComponents(components);

            yield return MeasureTime();
        }
        [UnityTest, Performance]
        public IEnumerator MeasureDynamicRaycastCone(
            [ValueSource(nameof(testComponents))] AdditionalComponents components) {
            yield return Wait.forFixedUpdate;

            spotlight = spotlightObj.AddComponent<PatrolSpotlight>();
            spotlight.rayLayers = 1 << LayerMask.NameToLayer("Foreground");
            spotlight.algorithm = PatrolSpotlight.VisionConeAlgorithm.DynamicRaycastCount;
            spotlight.rayCollider = levelCollider;

            AddAdditionalComponents(components);

            yield return MeasureTime();
        }
    }
}