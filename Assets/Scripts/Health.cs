using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [Header("Health Stats")]
    public int hP;
    public string damageString;
    public string deadString;
    public string hurtString;
    public bool invuln;
    public bool deadBool = false;
    public Sprite dead;
    [Header("Animations")]
    public Animator anim;

    [Header("Grounded Stats")]
    public bool grounded;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public AudioSource source;
    public AudioClip hurtClip;
    public AudioClip dieClip;
    // Use this for initialization
    void Start () {
        source = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update ()
    {
		if(hP<=0 && !deadBool)
        {
            deadBool = true;
            Death();
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName(damageString))
        {
            anim.SetBool("damage", false);
            invuln = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(hurtString))
        {
            anim.SetBool("damage", false);
        }

    }

    public void Death()
    {
        anim.SetBool("dead", true);
        this.GetComponent<SpriteRenderer>().sprite = dead;
        source.clip = dieClip;
        source.Play();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(deadString))
        {
            StartCoroutine(delete());
        }
    }
    public void Damaged(int damage)
    {
        source.clip = hurtClip;
        source.Play();
        if (!invuln)
        {
            hP -= damage;
            anim.SetBool("damage", true);

        }
    }


    public IEnumerator delete()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);

    }

    public void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if(grounded)
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
    }



}
