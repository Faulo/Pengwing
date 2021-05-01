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

        float currentWaitTime;
        bool facingRight;
        Waypoint[] waypoints;
        int currentWaypointIndex;

        void Awake() {
            OnValidate();
            waypoints = GetComponentsInChildren<Waypoint>();
        }
        void OnValidate() {
            if (!attachedAnimator) {
                attachedAnimator = GetComponentInChildren<Animator>();
            }
        }

        void Start() {
            facingRight = true;
            currentWaypointIndex = 0;
            currentWaitTime = waitTime;
        }

        void FixedUpdate() {
            // Visitor Bewegen
            attachedAnimator.transform.position = Vector2.MoveTowards(attachedAnimator.transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

            // Distanz zum aktuellen Waypoint checken
            if (Vector2.Distance(attachedAnimator.transform.position, waypoints[currentWaypointIndex].position) < reachDistance) {

                // Wenn genug gewartet wurde
                if (currentWaitTime <= 0) {

                    if (currentWaypointIndex < waypoints.Length - 1) {

                        currentWaypointIndex++;

                    } else {
                        currentWaypointIndex = 0;
                    }

                    currentWaitTime = waitTime;
                    attachedAnimator.SetBool("IsWaiting", false);

                    // Flipping the Gameobject

                    // wenn der Waypoint rechts vom Visitor ist und er nach links schaut dann Flip
                    if (attachedAnimator.transform.position.x < waypoints[currentWaypointIndex].position.x) {

                        if (!facingRight) {
                            Flip();
                        }
                    }
                    // wenn der Waypoint links vom Visitor ist und er nach rechts schaut dann Flip 

                    else {
                        if (facingRight) {
                            Flip();
                        }
                    }

                    // Wartezeit herunterzählen
                } else {
                    currentWaitTime -= Time.deltaTime;
                    attachedAnimator.SetBool("IsWaiting", true);
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