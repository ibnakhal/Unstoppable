using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;

    public float moveF;
    public float maxSpeed;
    public float jumpP;
    private bool jump;
    [SerializeField]
    private float jumpF;

    public float rollSpeed;
    public bool timerOn = false;
    public bool RollingL;
    public bool RollingR;
    public float rollTime;
    public bool Shifted = false;
    public bool invin = false;
    [SerializeField]
    private float timer;

    //public bool Dashing = false;
    public float DashSpeed;
    //public bool dTOn = false;
    public bool DLeft = false;
    public bool DRight = false;
    public float DTime;
    [SerializeField]
    private int dashAmount;
    public int totalDashes;

    [SerializeField]
    private bool attacking = false;
    public float att1Time;
    private float attTimer;

    public bool airAttack;
    public float aAAttTime;
    private float aATimer;

    private float timer2;
    public float dtInterval;
    public bool tapped = false;

    [SerializeField]
    private bool doubleJump;

    public enum AttackStates { first, second, third, rest};
    public AttackStates attst;
    public float att2Time;
    public float att3Time;
    [SerializeField]
    public float attDelay;

    public Transform groundCheck;
    public Transform groundCheck2;
    public Transform groundCheck3;

    [SerializeField]
    private bool isGrounded = false;

    private Animator anim;
    private Rigidbody2D rb2d;
    // Use this for initialization
    private void Awake()
    {
        
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = 0;
        anim.SetBool("rolling", false);
        anim.SetBool("dashing", false);
        dashAmount = totalDashes;
        timer2 = 0;
        attTimer = 0;
        RollingL = false;
        RollingR = false;
        tapped = false;
        airAttack = false;
        attst = AttackStates.rest;


    }


	
	// Update is called once per frame
	void Update () {
        if(Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, groundCheck3.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        attTimer -= Time.deltaTime;
        aATimer -= Time.deltaTime;
        if(attTimer <= -0.3f)
        {
            attst = AttackStates.rest;
        }
        if(isGrounded)
        {
            aATimer = 0;
        }
        if(attTimer > 0)
        {
            attacking = true;
        }
        else if(attTimer <= 0)
        {
            attacking = false;
        }

        if(aATimer > 0)
        {
            airAttack = true;
        }
        else if(aATimer <= 0)
        {
            airAttack = false;
        }
        if(attst == AttackStates.rest)
        {
            attacking = false;
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", false);
        }
        if(attst == AttackStates.first)
        {
            Vector2 d = rb2d.velocity;
            d.x = 0f;
            rb2d.velocity = d;
            attacking = true;
            anim.SetBool("hit1", true);
            anim.SetBool("hit2", false);
        }
        if(attst == AttackStates.second)
        {
            Vector2 d = rb2d.velocity;
            d.x = 0f;
            rb2d.velocity = d;
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", true);
        }
        if(attst == AttackStates.third)
        {
            Vector2 d = rb2d.velocity;
            d.x = 0f;
            rb2d.velocity = d;
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", true);
        }

        if(RollingL || RollingR)
        {
            attst = AttackStates.rest;
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", false);
        }

        if (!RollingL && !RollingR && Input.GetAxis("Fire1") > 0)
        {
            if(isGrounded && attst == AttackStates.rest)
            {
                attTimer = att1Time;
                attst = AttackStates.first;
                attacking = false;
            }
            if(isGrounded && attst == AttackStates.first)
            {
                if(attTimer <= attDelay)
                {
                    attTimer = att2Time;
                    attst = AttackStates.second;
                    
                }
            }
            if(isGrounded && attst == AttackStates.second)
            {
                if(attTimer <= attDelay)
                {
                    attTimer = att3Time;
                    attst = AttackStates.third;
                    
                }
            }
            if(isGrounded && attst == AttackStates.third)
            {
                if(attTimer <= attDelay)
                {
                    attst = AttackStates.rest;
                }
            }
            if(!isGrounded)
            {
                aATimer = aAAttTime;
            }
            
        }
        

        if(airAttack)
        {
            anim.SetBool("airAttacking", true);
        }
        if(!airAttack)
        {
            anim.SetBool("airAttacking", false);
        }
        if (tapped)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(isGrounded)
                {
                    timer = rollTime;
                    RollingL = true;
                    RollingR = false;

                    timerOn = true;
                    anim.SetBool("rolling", true);

                    timer2 = 0;
                }
                if(!isGrounded && dashAmount > 0)
                {
                    timer = DTime;
                    DLeft = true;

                    timerOn = true;
                    anim.SetBool("dashing", true);
                    dashAmount -= 1;
                }
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                if (isGrounded)
                {
                    timer = rollTime;
                    RollingR = true;
                    RollingL = false;

                    timerOn = true;
                    anim.SetBool("rolling", true);

                    timer2 = 0;
                }
                if (!isGrounded && dashAmount > 0)
                {
                    timer = DTime;
                    DRight = true;

                    timerOn = true;
                    anim.SetBool("dashing", true);
                    dashAmount -= 1;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
                timer2 = dtInterval;
                tapped = true;
        }

        timer2 -= Time.deltaTime;
        if(timer2 <= 0)
        {
            tapped = false;
        }
        

        
        if(Input.GetButtonDown ("Jump"))
        {
            
            jump = true;
        }
        anim.SetBool("grounded", isGrounded);
        if(isGrounded)
        {
            doubleJump = true;
            dashAmount = totalDashes;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            
            
                Shifted = true;
            
            
        }
        else
        {
            Shifted = false;
        }
        if(Shifted)
        {
            if(Input.GetKeyDown(KeyCode.A) && isGrounded)
            {
                timer = rollTime;
                RollingL = true;
                
                timerOn = true;
                anim.SetBool("rolling", true);
            }
            
            if(Input.GetKeyDown(KeyCode.D) && isGrounded)
            {
                timer = rollTime;
                RollingR = true;
                
                timerOn = true;
                anim.SetBool("rolling", true);
            }
            if(Input.GetKeyDown(KeyCode.A) && !isGrounded && dashAmount > 0)
            {
                timer = DTime;
                DLeft = true;
                
                timerOn = true;
                anim.SetBool("dashing", true);
                dashAmount -= 1;

            }
            if (Input.GetKeyDown(KeyCode.D) && !isGrounded && dashAmount > 0)
            {
                timer = DTime;
                DRight = true;
                
                timerOn = true;
                anim.SetBool("dashing", true);
                dashAmount -= 1;

            }
        }
        
        if(timer > 0 && timerOn)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0 && timerOn)
        {
            Vector2 v = rb2d.velocity;
            v.x = 0;
            rb2d.velocity = v;
            RollingL = false;
            RollingR = false;
            timerOn = false;
            //dTOn = false;
            DLeft = false;
            DRight = false;
            anim.SetBool("rolling", false);
            anim.SetBool("dashing", false);
        }
        float h = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(h));
        jumpF = Input.GetAxis("Vertical");



    }

    private void FixedUpdate()
    {
        if (!RollingL || !RollingR)
        {

            if (attst == AttackStates.rest)
            {


                float h = Input.GetAxis("Horizontal");


                if (h * rb2d.velocity.x < maxSpeed && !attacking)
                {
                    Vector2 v = rb2d.velocity;
                    v.x = h * moveF;
                    
                    rb2d.velocity = v;
                }
                if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                {
                    rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
                }
                if (h > 0 && !facingRight)
                {
                    if(!airAttack)
                    Flip();
                }

                else if (h < 0 && facingRight)
                {
                    if(!airAttack)
                    Flip();
                }
            }

        }
        if (jump && !attacking)
        {
            
            if (isGrounded)
            {
                rb2d.AddForce(new Vector2(0f, jumpP), ForceMode2D.Impulse);
                jump = false;
            }
            else if (doubleJump)
            {
                Vector2 j = rb2d.velocity;
                j.y = jumpP/5f;
                rb2d.velocity = j;
                //rb2d.AddForce(new Vector2(0f, jumpP), ForceMode2D.Impulse);
                jump = false;
                doubleJump = false;
                //restart animation
                anim.Play("Air", 0, 0f);
            }

        }
        
        if(RollingL)
        {
            float h = Input.GetAxis("Horizontal");
            if (h > 0 && !facingRight)
            {
                Flip();
            }

            else if (h < 0 && facingRight)
            {
                Flip();
            }
            Vector2 r = rb2d.velocity;
            if (timer > 0)
            {
                    
                    
                r.x = -1 * rollSpeed;
                rb2d.velocity = r;
            }
                
        }
        if (RollingR)
        {
            float h = Input.GetAxis("Horizontal");
            if (h > 0 && !facingRight)
            {
                Flip();
            }

            else if (h < 0 && facingRight)
            {
                Flip();
            }
            Vector2 r = rb2d.velocity;
            if (timer > 0)
            {
                    
                    
                r.x = 1 * rollSpeed;
                rb2d.velocity = r;
            }
                
        }
        if(DLeft)
        {
            if(timer > 0)
            {
                rb2d.velocity = new Vector2(-1 * DashSpeed, 0);
            }
        }
        if(DRight)
        {
            if(timer > 0)
            {
                rb2d.velocity = new Vector2(DashSpeed, 0);
            }
        }
        
        


    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
