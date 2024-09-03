using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Runtime.Level {
    [CreateAssetMenu(fileName = "LX_PlatformTile_New", menuName = "Tiles/Platform Tile", order = 100)]
    public class PlatformTile : TileBase {
        enum SpriteId {
            TopLeft,
            Top,
            TopRight,
            ColumnTop,
            Left,
            Center,
            Right,
            ColumnCenter,
            BottomLeft,
            Bottom,
            BottomRight,
            ColumnBottom,
            RowLeft,
            RowCenter,
            RowRight,
            Single
        }
        [Flags]
        enum SpritePosition {
            Top = 1 << 0,
            Right = 1 << 1,
            Bottom = 1 << 2,
            Left = 1 << 3
        }
        static readonly Dictionary<SpritePosition, SpriteId> idOverPosition = new() {
            [0] = SpriteId.Single,
            [SpritePosition.Bottom | SpritePosition.Right] = SpriteId.TopLeft,
            [SpritePosition.Bottom | SpritePosition.Left] = SpriteId.TopRight,
            [SpritePosition.Top | SpritePosition.Left] = SpriteId.BottomRight,
            [SpritePosition.Top | SpritePosition.Right] = SpriteId.BottomLeft,
            [SpritePosition.Left] = SpriteId.RowRight,
            [SpritePosition.Left | SpritePosition.Right] = SpriteId.RowCenter,
            [SpritePosition.Right] = SpriteId.RowLeft,
            [SpritePosition.Top] = SpriteId.ColumnBottom,
            [SpritePosition.Top | SpritePosition.Bottom] = SpriteId.ColumnCenter,
            [SpritePosition.Bottom] = SpriteId.ColumnTop,
            [SpritePosition.Top | SpritePosition.Bottom | SpritePosition.Left] = SpriteId.Right,
            [SpritePosition.Top | SpritePosition.Bottom | SpritePosition.Right] = SpriteId.Left,
            [SpritePosition.Top | SpritePosition.Left | SpritePosition.Right] = SpriteId.Bottom,
            [SpritePosition.Bottom | SpritePosition.Left | SpritePosition.Right] = SpriteId.Top,
            [SpritePosition.Top | SpritePosition.Bottom | SpritePosition.Left | SpritePosition.Right] = SpriteId.Center,
        };
        static readonly (SpritePosition, Vector3Int)[] offsetOverPosition = new[] {
            (SpritePosition.Top, Vector3Int.up),
            (SpritePosition.Left, Vector3Int.left),
            (SpritePosition.Bottom, Vector3Int.down),
            (SpritePosition.Right, Vector3Int.right),
        };

        [SerializeField, HideInInspector]
        Sprite[] sprites = default;
        [SerializeField]
        TileFlags tileOptions = TileFlags.LockAll;
        [SerializeField]
        Tile.ColliderType colliderType = Tile.ColliderType.None;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
            tilemap.RefreshTile(position);
            foreach (var (add, offset) in offsetOverPosition) {
                if (tilemap.GetTile(position + offset) == this) {
                    tilemap.RefreshTile(position + offset);
                }
            }
        }
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            var mask = CalculateSpritePosition(position, tilemap);
            tileData.sprite = LookupSprite(idOverPosition[mask]);
            tileData.flags = tileOptions;
            tileData.colliderType = colliderType;
        }
        SpritePosition CalculateSpritePosition(Vector3Int position, ITilemap tilemap) {
            SpritePosition mask = 0;
            foreach (var (add, offset) in offsetOverPosition) {
                if (tilemap.GetTile(position + offset) == this) {
                    mask |= add;
                }
            }

            return mask;
        }
        Sprite LookupSprite(SpriteId spriteId) {
            return sprites[(int)spriteId];
        }

#if UNITY_EDITOR
        [Header("Editor Tools")]
        [SerializeField, Expandable]
        Texture2D spriteSheet = default;
        protected void OnValidate() {
            if (spriteSheet) {
                sprites = UnityEditor.AssetDatabase
                    .LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(spriteSheet))
                    .OfType<Sprite>()
                    .OrderBy(sprite => int.Parse(Regex.Match(sprite.name, "\\d+$").Value))
                    .ToArray();
                Assert.AreEqual(sprites.Length, idOverPosition.Count, $"{this} requires exactly {idOverPosition.Count} sprites in {spriteSheet}!");
            }
        }
#endif
    }
}