using UnityEngine;

namespace Runtime.Player {
    public class AvatarController : MonoBehaviour {
        enum Parameters {
            isInWater,
            isAlive,
            intendsToJump,
            isGrounded,
            walkSpeed,
            movementSpeed,
            canFly,
            isWin,
        }
        public static AvatarController instance;
        [Header("MonoBehaviour")]
        [SerializeField]
        CharacterController attachedCharacter = default;
        [SerializeField]
        Animator attachedAnimator = default;
        [SerializeField]
        SpriteRenderer attachedRenderer = default;

        [Header("Physics")]
        [SerializeField]
        bool manualUpdate = false;
        [SerializeField]
        LayerMask spotlightLayer = default;
        [SerializeField]
        LayerMask goalLayer = default;
        [SerializeField]
        LayerMask waterLayer = default;
        [SerializeField]
        Vector3 waterOffset = Vector3.zero;
        [SerializeField, Range(0, 1)]
        float waterRadius = 1;
        [SerializeField]
        LayerMask swampLayer = default;
        [SerializeField]
        Vector3 swampOffset = Vector3.zero;
        [SerializeField, Range(0, 1)]
        float swampRadius = 1;
        [SerializeField, Range(-100, 100)]
        float outOfBoundsY = -10;

        [Header("Input")]
        [SerializeField, Range(0, 1)]
        float jumpBufferDuration = 1;

        [Header("Debug")]
        public Vector2 movementInput;
        public Vector2 velocity;
        public bool isInWater;
        public bool isInSwamp;
        public bool isAlive;
        public bool isFacingLeft;
        public bool isSeen;
        public bool canFly;
        public bool isWin;
        public float facingMultiplier => isFacingLeft
            ? -1
            : 1;

        float jumpTimer;

        void Awake() {
            OnValidate();
            isAlive = true;
            instance = this;
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
            UpdateJump();
            isInWater = Physics2D.OverlapCircle(transform.position + waterOffset, waterRadius, waterLayer);
            isInSwamp = transform.position.y < outOfBoundsY || Physics2D.OverlapCircle(transform.position + swampOffset, swampRadius, swampLayer);
            isSeen = Physics2D.OverlapCircle(transform.position, attachedCharacter.radius, spotlightLayer);
            if (isInSwamp) {
                isAlive = false;
            }
            var goal = Physics2D.OverlapCircle(transform.position, attachedCharacter.radius, goalLayer);
            if (goal && goal.TryGetComponent<GoalController>(out var g) && !isWin) {
                isWin = true;
                g.Win();
            }
            canFly = isAlive && !isSeen;

            attachedAnimator.SetBool(nameof(Parameters.isInWater), isInWater);
            attachedAnimator.SetBool(nameof(Parameters.isAlive), isAlive);
            attachedAnimator.SetBool(nameof(Parameters.isGrounded), attachedCharacter.isGrounded);
            attachedAnimator.SetBool(nameof(Parameters.canFly), canFly);
            attachedAnimator.SetFloat(nameof(Parameters.walkSpeed), Mathf.Abs(velocity.x));
            attachedAnimator.SetFloat(nameof(Parameters.movementSpeed), velocity.magnitude);
            attachedAnimator.SetBool(nameof(Parameters.isWin), isWin);

            attachedAnimator.enabled = !manualUpdate;
            if (manualUpdate) {
                attachedAnimator.Update(Time.deltaTime);
            }

            transform.rotation = isFacingLeft
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.identity;

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
        void UpdateJump() {
            if (jumpTimer > 0) {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer < 0) {
                    attachedAnimator.ResetTrigger(nameof(Parameters.intendsToJump));
                }
            }
        }
        public void Jump() {
            attachedAnimator.SetTrigger(nameof(Parameters.intendsToJump));
            jumpTimer = jumpBufferDuration;
        }
    }
}