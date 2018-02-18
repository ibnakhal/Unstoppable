using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [Header("Animations")]
    public SpriteRenderer masterSprite;
    public Animator anim;
   

    [Header("Movement Variables")]
    public float walkSpeed;
    private Rigidbody2D body;

    [Header("Jumping")]
    public float jumpSpeed;
    public bool grounded;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    private Vector2 velocity = Vector2.zero;

    [Header("State Bools")]
    public bool isIdle;
    public bool facingRight = true;


    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        masterSprite = this.GetComponent<SpriteRenderer>();
        body = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKey && Input.GetAxis("Horizontal") == 0 && grounded)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
    }

    public void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (Input.GetAxis("Jump") > 0)
        {
            if (grounded)
            {
                body.velocity = (new Vector2(0, jumpSpeed));
                grounded = !grounded;
                anim.SetBool("jump", true);
                anim.SetBool("isGrounded", false);
            }
        }
        if(grounded)
        {
            anim.SetBool("isGrounded", true);
        }

        float move = Input.GetAxis("Horizontal");

                body.velocity = new Vector2(move * walkSpeed, body.velocity.y);
          
            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }
    

    public void Flip()
    {
        facingRight = !facingRight;
        if (facingRight)
        {
            masterSprite.flipX = true;
        }
        else if (!facingRight)
        {
            masterSprite.flipX = false;
        }
    }

}
