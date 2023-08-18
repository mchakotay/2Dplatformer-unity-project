using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script is used for spikes that are attached to moving platforms
public class SpikedPlatform : MonoBehaviour
{
    //in unity, drag in the transform of the platform we are attatching it to
    [SerializeField] private Transform platform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { //set the transform of the spike, equal to the transform of the platform so that it moves with it
        transform.position = new Vector2(platform.position.x, platform.position.y + .75f); //we add .75 to y so its on top of the platform not inside
    }
    //there is PROBABLY a better way to do this. I think if we do it similar to how we do sticky platforms,
    //and set the transform as a child of the platform transform, I think I can still offset it on the y axis.
    //thats probably more efficient, but PROBABLY shouldn't cause performance issues
}
