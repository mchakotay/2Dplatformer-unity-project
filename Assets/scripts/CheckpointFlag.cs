using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player1")
        {
            PlayerMovement.respawnPoint = transform.position; //uhhh I need to check if this is still necessary, since we are also setting it in player movement code
            anim.SetTrigger("activated"); //this tells to change the checkpoint to activated
        }
    }
}
