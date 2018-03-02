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
    public float timer;
    public float attack1Delay;
    public float attack2Delay;
    public float attack3Delay;
    public bool charge;
    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        masterSprite = this.GetComponent<SpriteRenderer>();
        body = this.GetComponent<Rigidbody2D>();
        //StartCoroutine(attackReset());
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
        timer -= Time.deltaTime;
        //if(Input.GetAxis("Fire1")!=0)
        if (Input.GetMouseButtonDown(0))
        {
            attackCounter++;
            anim.SetInteger("attack", attackCounter);
            anim.SetBool("attacking", true);
            hitbox.GetComponent<Hitbox>().hit = true;
            hitbox.GetComponent<Hitbox>().damage = 2;
            timer = attack1Delay;
        }
        if (timer <= 0)
        {
            anim.SetBool("attacking", false);
            attackCounter = 0;
            anim.SetInteger("attack", attackCounter);
            timer = 0;
            charge = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("charge", true);
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerChargeFire")&& !charge)
        {
            charge = true;
        }
        if (charge)
        {
            hitbox.GetComponent<Hitbox>().hit = true;
            hitbox.GetComponent<Hitbox>().damage = 5;
            if (facingLeft)
            {
                //this.gameObject.transform.Translate(Vector2.right * walkSpeed *2* Time.deltaTime);
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * walkSpeed * 2000);
            }
            if(!facingLeft)
            {
                //this.gameObject.transform.Translate(Vector2.left * walkSpeed *2* Time.deltaTime);
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * walkSpeed * 2000);


            }
        }

        if (isIdle)
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
            clone.GetComponent<Shockwavemovement>().isleft = false;
            GameObject clone2 = Instantiate(shockWave, waveSpawn2.position, waveSpawn2.rotation);
            clone2.GetComponent<Shockwavemovement>().isleft = true;


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

    //public IEnumerator attackReset()
    //{
    //    while (isActiveAndEnabled)
    //    {
    //        yield return new WaitForSeconds(1);
    //        if (isIdle)
    //        {
    //            attackCounter = 0;
    //            anim.SetInteger("attack", attackCounter);
    //            anim.SetBool("attacking", false);
    //            hitbox.GetComponent<Hitbox>().hit = false;

    //        }
    //    }
    //}


    public IEnumerator wave()
    {
        yield return new WaitForSeconds(1);
        schocked = false;
    }



    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("boom");
        if (other.gameObject.tag == "Enemy")
        {
            if (charge)
            {
                charge = !charge;
                anim.SetBool("charge", false);
                hitbox.GetComponent<Hitbox>().hit = false;
            }
        }
    }
}

//use a timer instead of waiting for ticks. each time you attack reset the timer.
// time -= time.deltatime
