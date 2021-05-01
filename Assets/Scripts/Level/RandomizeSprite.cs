using System;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime.Level {
    public class RandomizeSprite : MonoBehaviour {
        [SerializeField, Expandable]
        SpriteRenderer attachedRenderer = default;
        [SerializeField, Expandable]
        Sprite[] sprites = Array.Empty<Sprite>();

        void Awake() {
            OnValidate();
            attachedRenderer.sprite = sprites.RandomElement();
        }
        void OnValidate() {
            if (!attachedRenderer) {
                attachedRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
}


