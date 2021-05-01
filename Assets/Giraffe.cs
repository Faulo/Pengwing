using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : MonoBehaviour
{
    public float downTime;
    float currentDownTime;

    public float upTime;
    float currentUpTime;

    public bool isDown;

    Animator anim;

   
    

    void Start()
    {
        isDown = false;
        currentUpTime = upTime;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // If Head is up
        if (!isDown) {
            
            if(currentUpTime <= 0) {

                isDown = true;
                anim.SetBool("IsDown", true);

                // set downtimer
                currentDownTime = downTime;

            } else {

                currentUpTime -= Time.fixedDeltaTime;

            }
        } 

        // If Head is down
        else {
            if (currentDownTime <= 0) {

                isDown = false;
                anim.SetBool("IsDown", false);

                // set uptimer
                currentUpTime = upTime;

            } else {

                currentDownTime -= Time.fixedDeltaTime;
            }

   
        }
        
    }
}
