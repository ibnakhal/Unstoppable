using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour {

    public string target;
    public bool attacking;
    [Header("Animations")]
    public Animator anim;
    public float timer;
    public float attackDelay;
   

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            anim.SetBool("attacking", false);
            attacking = false;
        }
    }


    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == target && !attacking)
        {
            attacking = true;
            anim.SetBool("attacking", true);
            this.GetComponentInParent<Health>().attackfunction(true);
            timer = attackDelay;
        }



    }
    public void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == target && attacking)
        {
            attacking = false;
            anim.SetBool("attacking", false);
            this.GetComponentInParent<Health>().attackfunction(false);
        }



    }




}
