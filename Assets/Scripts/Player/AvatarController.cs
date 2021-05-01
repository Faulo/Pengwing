using UnityEngine;

namespace Runtime.Player {
    public class AvatarController : MonoBehaviour {
        enum Parameters {
            isInWater,
            intendsToJump,
            isGrounded,
            walkSpeed,
            movementSpeed,
            canFly,
        }
        [Header("MonoBehaviour")]
        [SerializeField]
        CharacterController attachedCharacter = default;
        [SerializeField]
        Animator attachedAnimator = default;
        [SerializeField]
        SpriteRenderer attachedRenderer = default;

        [Header("Physics")]
        [SerializeField]
        LayerMask waterLayer = default;

        [Header("Debug")]
        public Vector2 movementInput;
        public Vector2 velocity;
        public bool isInWater;
        public bool isFacingLeft;
        public bool canFly = true;
        public float facingMultiplier => isFacingLeft
            ? -1
            : 1;

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
            if (!attachedRenderer) {
                TryGetComponent(out attachedRenderer);
            }
        }
        void FixedUpdate() {
            isInWater = Physics2D.OverlapCircle(transform.position, attachedCharacter.radius, waterLayer);

            attachedAnimator.SetBool(nameof(Parameters.isInWater), isInWater);
            attachedAnimator.SetBool(nameof(Parameters.isGrounded), attachedCharacter.isGrounded);
            attachedAnimator.SetBool(nameof(Parameters.canFly), canFly);
            attachedAnimator.SetFloat(nameof(Parameters.walkSpeed), Mathf.Abs(velocity.x));
            attachedAnimator.SetFloat(nameof(Parameters.movementSpeed), velocity.magnitude);
            attachedRenderer.flipX = isFacingLeft;

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