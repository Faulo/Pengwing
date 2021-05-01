using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Effects {
    [CreateAssetMenu]
    public class LoadScene : ScriptableObject {
        public void Invoke(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
    }
}