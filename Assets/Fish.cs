using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Sprite[] spriteList;
    public SpriteRenderer rend;
    public GameObject fish;

    [SerializeField, Range(0, 10)]
    float speed;
    [SerializeField, Range(0, 10)]
    float waitTime;
    [SerializeField, Range(0, 10)]
    float reachDistance;

    public Transform[] waypoints;

    public bool facingRight;

    float currentWaitTime;
    int currentWaypointIndex;

    int rand;

    void Start()
    {
        rand = Random.Range(0, spriteList.Length);
        rend.sprite = spriteList[rand];
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        facingRight = true;
        currentWaitTime = waitTime;
    }


    void FixedUpdate() {
        fish.transform.position = Vector2.MoveTowards(fish.transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.fixedDeltaTime);

        // Distanz zum aktuellen Waypoint checken
        if (Vector2.Distance(fish.transform.position, waypoints[currentWaypointIndex].position) < reachDistance) {

            // Wenn genug gewartet wurde
            if (currentWaitTime <= 0) {

                currentWaypointIndex = Random.Range(0, waypoints.Length);
                currentWaitTime = waitTime;


                // Flipping the Gameobject

                // wenn der Waypoint rechts vom Visitor ist und er nach links schaut dann Flip
                if (fish.transform.position.x < waypoints[currentWaypointIndex].position.x) {

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

            }
        }

    }

    public void Flip() {
        facingRight = !facingRight;
        fish.transform.Rotate(0f, 180f, 0f);
    }
}
