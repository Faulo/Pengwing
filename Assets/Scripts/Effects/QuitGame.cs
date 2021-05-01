using UnityEngine;

namespace Runtime.Effects {
    [CreateAssetMenu]
    public class QuitGame : ScriptableObject {
        public void Invoke() {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}