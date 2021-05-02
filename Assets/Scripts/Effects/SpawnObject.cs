using UnityEngine;

namespace Runtime.Effects {
    [CreateAssetMenu]
    public class SpawnObject : ScriptableObject {
        public void Invoke(GameObject prefab) {
            Instantiate(prefab);
        }
    }
}