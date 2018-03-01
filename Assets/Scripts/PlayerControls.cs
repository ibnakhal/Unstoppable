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
    public bool facingLeft = true;

    [Header ("Combat")]
    public int attackCounter;
    public GameObject shockWave;
    public Transform waveSpawn1;
    public Transform waveSpawn2;
    public bool schocked;
    public GameObject hitboxPivot;
    public GameObject hitbox;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        masterSprite = this.GetComponent<SpriteRenderer>();
        body = this.GetComponent<Rigidbody2D>();
        StartCoroutine(attackReset());
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

        if(Input.GetAxis("Fire1")!=0)
        {
            attackCounter++;
            anim.SetInteger("attack", attackCounter);
            anim.SetBool("attacking", true);
            hitbox.GetComponentInChildren<Hitbox>().hit = true;
        }


        if(isIdle)
        {
            anim.SetBool("idlestate", true);
        }
        else
        {
            anim.SetBool("idlestate", false);
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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLand") && !schocked)
        {
            schocked = true;
            GameObject clone = Instantiate(shockWave, waveSpawn1.position, waveSpawn1.rotation);
            GameObject clone2 = Instantiate(shockWave, waveSpawn2.position, waveSpawn2.rotation);

            StartCoroutine(wave());
        }

        float move = Input.GetAxis("Horizontal");

                body.velocity = new Vector2(move * walkSpeed, body.velocity.y);
        if (move != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
            if (move > 0 && !facingLeft)
            {
                Flip();
            }
            else if (move < 0 && facingLeft)
            {
                Flip();
            }
        }
    

    public void Flip()
    {
        facingLeft = !facingLeft;
        if (facingLeft)
        {
            hitboxPivot.transform.Rotate(new Vector3(0, 180, 0));
          masterSprite.flipX = false;
        }
        else if (!facingLeft)
        {
            hitboxPivot.transform.Rotate(new Vector3(0, 180, 0));

            masterSprite.flipX = true;
        }
    }

    public IEnumerator attackReset()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(3);
            if (isIdle)
            {
                attackCounter = 0;
                anim.SetInteger("attack", attackCounter);
                anim.SetBool("attacking", false);
                hitbox.GetComponentInChildren<Hitbox>().hit = false;

            }
        }
    }


    public IEnumerator wave()
    {
        yield return new WaitForSeconds(1);
        schocked = false;
    }
}
