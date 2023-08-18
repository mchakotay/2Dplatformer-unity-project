using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //declaring components that are on the player object
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    //layer masks that we use in isGrounded ()method
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask hazard; //currently defunct in my code

    //audio source for the jump sound effect
    [SerializeField] private AudioSource jumpSoundEffect;

    //respawn point (checkpoint location)
    public static Vector3 respawnPoint;

    private float dirX = 0f; //horizontal movement input value
    [SerializeField] private float moveSpeed = 8.26f; //player move speed, we can tweak from inside unity
    [SerializeField] private float jumpForce = 10.57f; //player jump force, we can tweak from inside unity

    private enum MovementState { idle, running, jumping, falling  } //an enumerated list of possible animation states

    private float coyoteTime = 0.15f; //the time that the player has after falling off a platform where a jump will still execute successfully
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.15f; //the amount of time before the player lands that they can input a jump and it will execute as soon as they hit the ground
    private float jumpBufferCounter;
    // Start is called before the first frame update
    //declaring double jump variables

    private bool hasDoubleJump;
    private bool doubleJumpUnlocked = true;

    private void Start() //start is ran when the scene initally loads
    {
        //here we get all the necessary components from the player object
        //and set the initial spawn point to where its placed in the editor (i.e. start of level)
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal"); //get our horizontal input (a value between 1(right) and -1(left))
        
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); //set our horizontal velocity = to our horizontal input times our movespeed

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime; //if our player is grounded, set the coyotetime timer to 0.15
            hasDoubleJump = true;
        }
        else //if the player is not grounded(falling/jumping) every frame we subtract a unit of time off of coyote time(this creates a countdown)
        {
            coyoteTimeCounter -= Time.deltaTime; //we use time.deltaTime because it has something to do with frames
        }                                        //we want time to be consistent if its running at 30fps or 60fps

        if (Input.GetButtonDown("Jump"))
        { //if the player presses jump, see the buffer counter = to 0.15
            jumpBufferCounter = jumpBufferTime; 
        }
        else
        { //if were not pressing jump, we start counting down from 0.15
            jumpBufferCounter -= Time.deltaTime; 
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        { //if our jump buffer and coyote time is greater than 0, then jump
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f; //we set jump buffer counter to 0 after a jump so that we cannot jump infinitely in the air
        }

        if (Input.GetButtonDown("Jump") && hasDoubleJump && doubleJumpUnlocked && !IsGrounded())
        { //if the player presses jump, has a double jump available, has unlocked doublejump, and is in the air, then double jump
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            hasDoubleJump = false; //after they jump once, set to false so they can't double jump again
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        { //if we let go of jump in the air and we are still moving up, then we start slowing down the players vertical velocity
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            //this results in holding a for longer jumps, and tapping a for short jumps
            coyoteTimeCounter = 0f; //we set coyote time counter to zero
            //trying to think about why...
            //I think this is also to prevent infinite double jumps
            //because if we are still moving up, and we let go of jump to press jump again,
            //then as soon as we let go of jump, then we set to zero so that we cant perform another successful jump
        }

        UpdateAnimationState(); //at the end of our update block, we call update animation state to make sure we are using right animation
    }
    private void OnCollisionEnter2D(Collision2D collision)
    { //when we collide with a checkpoint object, set that as our new respawn point
        if (collision.gameObject.CompareTag("checkpoint"))
        {
            respawnPoint = transform.position;
        }
    }

    private void RestartLevel()
    { //restart level is called by the death animation
        transform.position = respawnPoint; //set the players position to respawnPoint
        rb.bodyType = RigidbodyType2D.Dynamic; //set the rigid body type back to dynamic (we set it to static when we die so player cannot move while death animation is playing)
        UpdateAnimationState(); //I think I had to add this so that it would reset the player back to the right animation state(otherwise it would be stuck in death)
    }
    private void UpdateAnimationState() //called at the end of every frame update
    {
        MovementState state; //initialize our enumerated variable that holds our animation state

        if (dirX > 0f)
        { //if we are moving right, set animation state to running and don't flip sprite
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        { //if we are moving left, set animation state to running and flip the sprite
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        { //if we are not moving, set animation state to idle
            state = MovementState.idle; 
        }

        if (rb.velocity.y > .1f) //bc unity physics is wonky, I think we have to do 0.1f because its always greater than 0 (probably wonky shit with gravity)
        { //if we are moving up, set animation state to jumping
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        { //if we are moving down, set movement state to falling
            state = MovementState.falling;
        }
        //we use the animator component, call the setInteger method on the state variable, typecast our enumerated value to int
        //(I think we typecast because the animator doesn't have enumerated data type available)
        anim.SetInteger("state", (int)state);
        //SetInteger("var", value)
        //var is the name of the variable we are setting
        //value is what we are setting it to
    }

    private bool IsGrounded()
    { //I need to take another look at this later to dig into it
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .01f, jumpableGround);
        //doing a quick read of documentation I think it works like this
        //this "casts" a box to detect collision for the physics system
        //it casts a box shaped collider, returning the first collider in the scene it touches
        //first argument is the center of the box collider on the player
        //second argument is the size of the box collider, we set it to same size as box collider on player
        //third argument is the angle of the box being cast to detect collision (360 degrees, so 0 means = to direction)
        //fourth argument is the direction of the box being cast to detect collision
        //fifth argument is the max distance which to cast the box, it probably is 0.1f for wonky unity physics reasons
        //sixth argument is a layer mask, we are setting it to only detect colliders that are on the ground layer
    }
}
