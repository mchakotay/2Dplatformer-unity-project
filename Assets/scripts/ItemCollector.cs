using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This is the script for the collectibles counter in the top left of the screen
//This will probably get removed or repurposed later in development
//uses UI package namespace
//used on the player object

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;

    //This is a text object(we use the cherries text object thats sitting on the canvas object)
    [SerializeField] private Text cherriesText; 

    //This is the collection sound effect from the audio source
    [SerializeField] private AudioSource collectionSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    { //if the game object we collide with is a trigger, and has the cherry tag, then
        if(collision.gameObject.CompareTag("cherry"))
        {  
            collectionSoundEffect.Play(); // play the collection sound
            Destroy(collision.gameObject); //destroy the cherry object
            cherries++; //increment cherries counter
            cherriesText.text = "Cherries: " + cherries; //update the text
        }
    }
}
