using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script is essentially the exact same as waypoint follower, except we are flipping the sprite
//the possum is just looping its walking animation, no need to change animation states
public class OpossumMovement : MonoBehaviour
{
    //declare our variables
    private SpriteRenderer sprite;
    private int currentWaypointIndex = 0;

    [SerializeField] private GameObject[] waypoints; //we are declaring an array of game objects
    [SerializeField] private float speed = 2f; //the move speed of the platform, tweak in unity interface
    void Start()
    { //initialize the sprite component
        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        { //if the distance between the current waypoint and the player is less than .1(always greater than 0 bc wonky unity)
            currentWaypointIndex++; //we increment our waypoint index, meaning we start tracking the next waypoint in the arrays transform
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; //if the waypoint index is greater than the length of the waypoint array, set the index back to 0
            }
            sprite.flipX = !sprite.flipX; //set the sprite flip value to opposite so it turns when it changes direction
        }
        //this function moves our enemy to the next waypoint
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
        //first argument, the current position of the enemy
        //second argument, the position of the target waypoint
        //third argument, the speed variable times Time.deltaTime, or maxDistanceDelta
        //Time.deltaTime is the interval in seconds from the last frame to the current one.
        //speed * time = distance moved
        //for 60fps vs. 30fps, update will be called 60 times a second, not 30
        //this also means at 60fps, value for Time.deltaTime will be smaller
        //meaning that the distance moved will be smaller
        //this means that the distance moved will be consistent across framerates

        //side note: the documentation doesn't show it, but I imagine the MoveTowards function works something like this
        //take x and y coordinates of current, subtract that from x and y coordinates of target,
        //add or subtract our maxdistanceDelta to current x and y coordinates (i guess it just depends if its moving left, right, up, or down if we need to add or subtract)
    }
}
