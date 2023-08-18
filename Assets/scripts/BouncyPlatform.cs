using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    [SerializeField] private float bounciness = 3f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "player1")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounciness, ForceMode2D.Impulse);
            anim.SetBool("platbounced", true);
        }
    }
    private void waitForATime()
    {
        anim.SetBool("platbounced", false);
    }
}
