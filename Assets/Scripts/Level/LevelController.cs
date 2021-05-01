using System.Linq;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Level {
    public class LevelController : MonoBehaviour {
        enum CollisionMode {
            None,
            Collider,
            Trigger
        }
        [SerializeField]
        Grid attachedGrid = default;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedGrid) {
                TryGetComponent(out attachedGrid);
            }
        }
#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(LevelController))]
        class LevelControllerEditor : RuntimeEditorTools<LevelController> {
            protected override void DrawEditorTools() {
                DrawButton("(Re-)create tilemaps", target.ValidateTilemaps);
            }
        }
        void ValidateTilemaps() {
            RecreateTilemap("Background", 0, CollisionMode.None, LayerMask.NameToLayer("Background"));
            RecreateTilemap("Midground", 1, CollisionMode.None, LayerMask.NameToLayer("Midground"));
            RecreateTilemap("Foreground_BehindWater", 2, CollisionMode.Collider, LayerMask.NameToLayer("Foreground"));
            RecreateTilemap("Foreground_Water", 3, CollisionMode.Trigger, LayerMask.NameToLayer("Water"));
            RecreateTilemap("Foreground_InFrontOfWater", 4, CollisionMode.Collider, LayerMask.NameToLayer("Foreground"));
            RecreateTilemap("Foreground_Details", 5, CollisionMode.None, LayerMask.NameToLayer("Foreground"));
        }
        void RecreateTilemap(string name, int order, CollisionMode mode, int layer) {
            name = $"L{order}_{name}";
            var tilemap = GetComponentsInChildren<Tilemap>()
                .FirstOrDefault(t => t.name == name);
            if (!tilemap) {
                tilemap = new GameObject(name).AddComponent<Tilemap>();
                tilemap.transform.parent = transform;
            }
            tilemap.gameObject.isStatic = true;
            tilemap.gameObject.layer = layer;
            var renderer = tilemap.gameObject.GetOrAddComponent<TilemapRenderer>();
            renderer.sortingOrder = order;
            switch (mode) {
                case CollisionMode.None:
                    break;
                case CollisionMode.Collider: {
                    var collider = tilemap.gameObject.GetOrAddComponent<TilemapCollider>();
                    break;
                }
                case CollisionMode.Trigger: {
                    var collider = tilemap.gameObject.GetOrAddComponent<TilemapCollider2D>();
                    var composite = tilemap.gameObject.GetOrAddComponent<CompositeCollider2D>();
                    var rigidbody = tilemap.gameObject.GetOrAddComponent<Rigidbody2D>();
                    collider.isTrigger = true;
                    collider.usedByComposite = true;
                    composite.isTrigger = true;
                    composite.generationType = CompositeCollider2D.GenerationType.Synchronous;
                    composite.geometryType = CompositeCollider2D.GeometryType.Polygons;
                    rigidbody.bodyType = RigidbodyType2D.Static;
                    break;
                }
                default:
                    break;
            }
            UnityEditor.EditorUtility.SetDirty(tilemap);
        }
#endif
    }
}