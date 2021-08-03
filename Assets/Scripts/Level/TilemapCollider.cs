using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Runtime.Level {
    public class TilemapCollider : MonoBehaviour {
        [SerializeField]
        Tilemap attachedTilemap;
        [SerializeField]
        MeshCollider attachedCollider;
        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedTilemap) {
                TryGetComponent(out attachedTilemap);
            }
            if (!attachedCollider) {
                attachedCollider = gameObject.GetOrAddComponent<MeshCollider>();
            }
        }
        void Start() {
            BakeTilemap();
        }
        void BakeTilemap() {
            Assert.IsTrue(attachedTilemap);
            Assert.IsTrue(attachedCollider);

            var cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cubeMesh = cubeObject.GetComponent<MeshFilter>().sharedMesh;
            Destroy(cubeObject);

            Assert.IsTrue(cubeMesh);

            if (attachedCollider.sharedMesh) {
                Destroy(attachedCollider.sharedMesh);
            }

            var meshes = new List<CombineInstance>();
            foreach (var position in attachedTilemap.cellBounds.allPositionsWithin) {
                if (attachedTilemap.HasTile(position)) {
                    var tile = attachedTilemap.GetTile(position);
                    var type = attachedTilemap.GetColliderType(position);
                    switch (type) {
                        case Tile.ColliderType.None:
                        case Tile.ColliderType.Sprite:
                            break;
                        case Tile.ColliderType.Grid:
                            var worldPosition = attachedTilemap.CellToWorld(position) + attachedTilemap.tileAnchor;
                            var tileTransform = attachedTilemap.GetTransformMatrix(position);
                            tileTransform.SetTRS(worldPosition, tileTransform.rotation, Vector3.one);
                            var instance = new CombineInstance {
                                mesh = cubeMesh,
                                transform = tileTransform,
                            };
                            meshes.Add(instance);
                            break;
                        default:
                            throw new NotImplementedException(type.ToString());
                    }
                }
            }
            var mesh = new Mesh {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };
            mesh.CombineMeshes(meshes.ToArray(), true, true, false);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.Optimize();
            attachedCollider.sharedMesh = mesh;
        }
    }
}