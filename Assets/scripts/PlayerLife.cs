using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//on the player object, I was using scene management package namespace to reload the scene,
//but I didnt like how it worked, so I changed it when designing the checkpoint system
public class PlayerLife : MonoBehaviour
{
    //declare our variables
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource deathSoundEffect; //drag audio clips in here from unity editor

    private void Start()
    { //initialize our variables to components on player
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    { //if the player, collides with an object, and it has the trap tag, then call Die() function
        if (collision.gameObject.CompareTag("trap"))
        {
            Die();
        }
    }

    private void Die()
    { //When the player dies we want to,
        deathSoundEffect.Play(); //play the death sound effect
        rb.bodyType = RigidbodyType2D.Static; //set the player object to static, so the player cannot move after he dies
        anim.SetTrigger("death"); //we set the "death" trigger on animator (there has got to be a better way to do this)
    }
}
