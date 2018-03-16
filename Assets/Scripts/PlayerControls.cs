﻿using System.Collections;
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
    public bool moving;

    [Header("Jumping")]
    public float jumpSpeed;
    public bool grounded;
    public bool jumped;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    private Vector2 velocity = Vector2.zero;
    public AudioClip jumpClip;
    public AudioClip landclip;
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
    public string attack1;
    public string attack2;
    public string attack3;


    public AudioSource source;
    // Use this for initialization
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();
        masterSprite = this.GetComponent<SpriteRenderer>();
        body = this.GetComponent<Rigidbody2D>();
        //moving = true;

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
        if (Input.GetKeyDown(attack1))
        {
            //moving = false;
            attackCounter++;
            anim.SetInteger("attack", attackCounter);
            anim.SetBool("attack1", true);
            hitbox.GetComponent<Hitbox>().hit = true;
            hitbox.GetComponent<Hitbox>().damage = 2;
            timer = attack1Delay;
            hitbox.GetComponent<Hitbox>().left = facingLeft;
            hitbox.GetComponent<Hitbox>().upMod = 100;

        }
        if (Input.GetKeyDown(attack2))
        {
            //moving = false;
            attackCounter++;
            anim.SetInteger("attack", attackCounter);
            anim.SetBool("attack2", true);
            hitbox.GetComponent<Hitbox>().hit = true;
            hitbox.GetComponent<Hitbox>().damage = 2;
            timer = attack2Delay;
            hitbox.GetComponent<Hitbox>().left = facingLeft;
            hitbox.GetComponent<Hitbox>().upMod = 3000;

        }
        if (Input.GetKeyDown(attack3))
        {
            //moving = false;
            attackCounter++;
            anim.SetInteger("attack", attackCounter);
            anim.SetBool("attack3", true);
            hitbox.GetComponent<Hitbox>().hit = true;
            hitbox.GetComponent<Hitbox>().damage = 2;
            timer = attack3Delay;
            hitbox.GetComponent<Hitbox>().left = facingLeft;
            hitbox.GetComponent<Hitbox>().upMod = 100;
        }
        if (timer <= 0)
        {
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
            attackCounter = 0;
            anim.SetInteger("attack", attackCounter);
            timer = 0;
            charge = false;
            hitbox.GetComponent<Hitbox>().hit = false;
            //moving = true;


        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("punch1"))
        {
            anim.SetBool("attack1", false);
            //moving = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("punch2"))
        {
            anim.SetBool("attack2", false);
            //moving = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("punch3"))
        {
            anim.SetBool("attack3", false);
            //moving = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
            hitbox.GetComponent<Hitbox>().upMod = 1500;

            if (facingLeft)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * walkSpeed * 2000);
            }
            if(!facingLeft)
            {
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
            if (grounded && !jumped)
            {
                body.velocity = (new Vector2(0, jumpSpeed));
                grounded = !grounded;
                anim.SetBool("jump", true);
                anim.SetBool("isGrounded", false);
                source.clip = jumpClip;
                source.Play();
                jumped = true;
            }
        }
        if (grounded)
        {
            anim.SetBool("isGrounded", true);

        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLand") && !schocked)
        {
            source.clip = landclip;
            source.Play();
            schocked = true;
            GameObject clone = Instantiate(shockWave, waveSpawn1.position, waveSpawn1.rotation);
            clone.GetComponent<Shockwavemovement>().isleft = false;
            clone.GetComponent<Hitbox>().left = true;
            GameObject clone2 = Instantiate(shockWave, waveSpawn2.position, waveSpawn2.rotation);
            clone2.GetComponent<Shockwavemovement>().isleft = true;
            jumped = false;

            StartCoroutine(wave());
        }
        //if (moving)
        //{
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
        //}
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



    public IEnumerator wave()
    {
        yield return new WaitForSeconds(1);
        schocked = false;
    }



    public void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 dir = new Vector2();

        if(facingLeft)
        {
            dir = Vector2.left;
        }
        else
        {
            dir = Vector2.right;
        }

        Debug.Log("boom");
        if (other.gameObject.tag == "Enemy")
        {
            if (charge)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector2.up*2000)+ dir);
                charge = !charge;
                anim.SetBool("charge", false);
                hitbox.GetComponent<Hitbox>().hit = false;
            }
        }
    }
}

//use a timer instead of waiting for ticks. each time you attack reset the timer.
// time -= time.deltatime
