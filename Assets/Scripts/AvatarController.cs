using UnityEngine;

namespace Runtime {

    public class AvatarController : MonoBehaviour {
        [SerializeField]
        CharacterController attachedCharacter = default;
        [SerializeField, Range(0, 100)]
        float movementSpeed = 10;
        [SerializeField, Range(0, 100)]
        float jumpSpeed = 10;
        [SerializeField, Range(0, 10)]
        float waterBreakDuration = 1;

        public bool intendsToJump;
        public Vector2 movementInput;
        Vector2 velocity = Vector2.zero;
        Vector2 acceleration;

        bool isInWater;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedCharacter) {
                TryGetComponent(out attachedCharacter);
            }
        }
        void FixedUpdate() {
            if (isInWater) {
                velocity = Vector2.SmoothDamp(velocity, movementSpeed * movementInput, ref acceleration, waterBreakDuration);
            } else {
                if (attachedCharacter.isGrounded) {
                    velocity.y = 0;
                    if (intendsToJump) {
                        intendsToJump = false;
                        velocity.y = jumpSpeed;
                    }
                } else {
                    velocity += Physics2D.gravity * Time.deltaTime;
                }
                velocity.x = (movementSpeed * movementInput).x;
            }
            attachedCharacter.Move(velocity * Time.deltaTime);
        }
        void OnControllerColliderHit(ControllerColliderHit hit) {
        }
        void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<Water>(out var water)) {
                isInWater = true;
            }
        }
        void OnTriggerStay(Collider other) {
        }
        void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<Water>(out var water)) {
                isInWater = false;
            }
        }
    }
}