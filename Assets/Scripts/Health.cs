using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [Header("Health Stats")]
    public int hP;
    public string damageString;
    public string deadString;
    public bool invuln;
    public Sprite dead;
    [Header("Animations")]
    public Animator anim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(hP<=0)
        {
            Death();
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName(damageString))
        {
            anim.SetBool("damage", false);
            invuln = false;
        }


    }

    public void Death()
    {
        anim.SetBool("dead", true);
        this.GetComponent<SpriteRenderer>().sprite = dead;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(deadString))
        {
            StartCoroutine(delete());
        }
    }
    public void Damaged(int damage)
    {
        if (!invuln)
        {
            hP -= damage;
            anim.SetBool("damage", true);
        }
    }


    public IEnumerator delete()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }




}
