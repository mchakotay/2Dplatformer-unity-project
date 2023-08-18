using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    //we have two box colliders on the sticky platforms
    //the one on top is a trigger, this is the one we are interacting with in this code
    //the bottom one is not a trigger, so this code does not run when we interact with that collider
    private void OnTriggerEnter2D(Collider2D collision)
    { //when the player collides with the top of the platform,
      //we set the players transform = to the transform of the platform
      //(we actually make the players transform a CHILD of the platforms transform)
      //(this method probably is much more efficient since we aren't having to copy the value over each time)
      //(likely since its a child its probably just pointing to the same place in memory)
      //this is what lets the player ride the platform as it moves
        if (collision.gameObject.name == "player1")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //then of course, when we exit the platform we set the parent to null
        if (collision.gameObject.name == "player1")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
