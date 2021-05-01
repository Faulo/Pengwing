using UnityEngine;

namespace Runtime.Level {
    public class Fish : MonoBehaviour {
        public Sprite[] spriteList;
        public SpriteRenderer rend;

        int rand;

        void Start() {
            rand = Random.Range(0, spriteList.Length);
            rend.sprite = spriteList[rand];
        }


    }
}


