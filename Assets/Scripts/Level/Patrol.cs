using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime.Level {
    public class Patrol : MonoBehaviour {
        [SerializeField, Expandable]
        Animator attachedAnimator = default;
        [SerializeField, Range(0, 10)]
        float speed = 2;
        [SerializeField, Range(0, 10)]
        float waitTime = 5;
        [SerializeField, Range(0, 10)]
        float reachDistance = 1;

        public bool pickWaypointsRandom;

        float currentWaitTime;
        bool facingRight;
        Waypoint[] waypoints;
        int currentWaypointIndex;

        protected void Awake() {
            OnValidate();
            waypoints = GetComponentsInChildren<Waypoint>();
        }
        protected void OnValidate() {
            if (!attachedAnimator) {
                attachedAnimator = GetComponentInChildren<Animator>();
            }
        }

        protected void Start() {
            facingRight = true;
            currentWaypointIndex = 0;
            currentWaitTime = waitTime;
            LookAtWaypoint();
        }

        protected void FixedUpdate() {
            // Visitor Bewegen
            attachedAnimator.transform.position = Vector2.MoveTowards(attachedAnimator.transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

            // Distanz zum aktuellen Waypoint checken
            if (Vector2.Distance(attachedAnimator.transform.position, waypoints[currentWaypointIndex].position) < reachDistance) {
                // Wenn genug gewartet wurde
                if (currentWaitTime < 0) {
                    if (pickWaypointsRandom) {
                        currentWaypointIndex = Random.Range(0, waypoints.Length);
                    } else {
                        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    }

                    currentWaitTime = waitTime;
                    attachedAnimator.SetBool("IsWaiting", false);

                    // Flipping the Gameobject
                    LookAtWaypoint();

                    // Wartezeit herunterzählen
                } else {
                    currentWaitTime -= Time.deltaTime;
                    attachedAnimator.SetBool("IsWaiting", true);
                }
            }
        }

        void LookAtWaypoint() {
            // wenn der Waypoint rechts vom Visitor ist und er nach links schaut dann Flip
            if (attachedAnimator.transform.position.x < waypoints[currentWaypointIndex].position.x) {
                if (!facingRight) {
                    Flip();
                }
            } else {
                // wenn der Waypoint links vom Visitor ist und er nach rechts schaut dann Flip 
                if (facingRight) {
                    Flip();
                }
            }
        }

        // Rotiert das Gameobject
        public void Flip() {
            facingRight = !facingRight;
            attachedAnimator.transform.Rotate(0f, 180f, 0f);
        }
    }
}