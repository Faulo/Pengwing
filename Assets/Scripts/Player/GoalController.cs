using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Player {
    public class GoalController : MonoBehaviour {
        public UnityEvent onWin;
        public void Win() {
            onWin.Invoke();
        }
    }
}