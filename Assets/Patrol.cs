using UnityEngine;

public class Patrol : MonoBehaviour {

    public float speed;
    public float waitTime;
    float currentWaitTime;

    public bool facingRight;

    public float reachDistance;

    public Transform[] waypoints;
    public int currentWaypoint;

    Animator anim;


    void Start() {
        facingRight = true;
        currentWaypoint = 0;
        currentWaitTime = waitTime;
        anim = GetComponent<Animator>();
    }


    void FixedUpdate() {
        // Visitor Bewegen
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.fixedDeltaTime);

        // Distanz zum aktuellen Waypoint checken
        if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < reachDistance) {

            // Wenn genug gewartet wurde
            if (currentWaitTime <= 0) {

                if (currentWaypoint < waypoints.Length - 1) {

                    currentWaypoint++;
                   
                } else {
                    currentWaypoint = 0;
                }

                currentWaitTime = waitTime;
                anim.SetBool("IsWaiting", false);

                // Flipping the Gameobject

                // wenn der Waypoint rechts vom Visitor ist und er nach links schaut dann Flip
                if (transform.position.x < waypoints[currentWaypoint].position.x) {

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
                currentWaitTime -= Time.fixedDeltaTime;
                anim.SetBool("IsWaiting", true);
            }

        }

    }

    // Rotiert das Gameobject
    public void Flip() {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

}
