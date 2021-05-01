using UnityEngine;

public class Patrol : MonoBehaviour {

    public float speed;
    public float waitTime;
    float currentWaitTime;

    public float reachDistance;

    public Transform[] waypoints;
    public int currentWaypoint;


    void Start() {
        currentWaypoint = 0;
        currentWaitTime = waitTime;
    }


    void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < reachDistance) {
            if (currentWaitTime <= 0) {

                if (currentWaypoint < waypoints.Length - 1) {

                    currentWaypoint++;

                } else {
                    currentWaypoint = 0;
                }

            } else {
                currentWaitTime -= Time.fixedDeltaTime;
            }

        }

    }
}
