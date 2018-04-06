using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleHealth : MonoBehaviour {

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
    public float speed;
    [Header("Grounded Stats")]
    public bool grounded;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public AudioSource source;
    public AudioClip hurtClip;
    public AudioClip dieClip;


    public enum State
    {
        walking,
        idle,
        attacking,
        hurt,
        END,
    }
    public State status;

    // Use this for initialization
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        //status = State.idle;

    }

    // Update is called once per frame
    void Update()
    {
        if (hP <= 0 && !deadBool)
        {
            deadBool = true;
            Death();
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(damageString) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            anim.SetBool("damage", false);
            invuln = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(hurtString))
        {
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
            anim.SetBool("damage", false);
        }


        switch (status)
        {
            case State.walking:
                anim.SetBool("walking", true);
                anim.SetBool("attacking", false);
                anim.SetBool("damage", false);
                anim.SetBool("isIdle", false);
                break;
            case State.idle:
                anim.SetBool("isIdle", true);
                anim.SetBool("attacking", false);
                anim.SetBool("damage", false);
                anim.SetBool("walking", false);
                break;
            case State.attacking:
                anim.SetBool("attacking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("walking", false);
                anim.SetBool("damage", false);
                break;
            case State.hurt:
                anim.SetBool("damage", true);
                anim.SetBool("attacking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("walking", false);

                break;
        }
        if (status == State.walking)
        {
            this.gameObject.transform.Translate(Vector2.left * Time.deltaTime * speed);
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
    public void Damage(int damage)
    {
        source.clip = hurtClip;
        source.Play();

            hP -= damage;
            status = State.hurt;
            anim.SetInteger("health", hP);
    }


    public IEnumerator delete()
    {
        Debug.Log(this.gameObject + "Died");
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);

    }

   


    public void attackfunction(bool swit)
    {
        if (swit)
        {
            status = State.attacking;
        }
        else if (!swit)
        {
            status = State.idle;
        }
    }
    public void idleFunction(bool swit)
    {
        if (swit)
        {
            status = State.idle;
        }
        else if (!swit)
        {
            status = State.walking;
        }
    }


}
