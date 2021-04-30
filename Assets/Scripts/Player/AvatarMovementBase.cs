using UnityEngine;

namespace Runtime.Player {
    public abstract class AvatarMovementBase : ScriptableObject {
        public abstract void EnterMovement(AvatarController avatar);
        public abstract void UpdateMovement(AvatarController avatar);
    }
}