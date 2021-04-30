using UnityEngine;

namespace Runtime.Player {
    public class AvatarController : MonoBehaviour {
        enum Parameters {
            isInWater,
            intendsToJump,
            isGrounded,
        }
        [Header("MonoBehaviour")]
        [SerializeField]
        CharacterController attachedCharacter = default;
        [SerializeField]
        Animator attachedAnimator = default;

        [Header("Physics")]
        [SerializeField]
        LayerMask waterLayer = default;

        [Header("Debug")]
        public Vector2 movementInput;
        public Vector2 velocity;
        public bool isInWater;

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedCharacter) {
                TryGetComponent(out attachedCharacter);
            }
            if (!attachedAnimator) {
                TryGetComponent(out attachedAnimator);
            }
        }
        void FixedUpdate() {
            isInWater = Physics.CheckSphere(transform.position, attachedCharacter.radius, waterLayer);

            attachedAnimator.SetBool(nameof(Parameters.isInWater), isInWater);
            attachedAnimator.SetBool(nameof(Parameters.isGrounded), attachedCharacter.isGrounded);

            attachedCharacter.Move(velocity * Time.deltaTime);
            if (attachedCharacter.isGrounded) {
                velocity.y = 0;
            }
        }
        void OnControllerColliderHit(ControllerColliderHit hit) {
        }
        void OnTriggerEnter(Collider other) {
        }
        void OnTriggerStay(Collider other) {
        }
        void OnTriggerExit(Collider other) {
        }
        public void Jump() {
            attachedAnimator.SetTrigger(nameof(Parameters.intendsToJump));
        }
    }
}