using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

    public Health hp;


	// Use this for initialization
	void Start () {
        hp = this.GetComponentInParent<Health>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hp.idleFunction(false);
        }
        hp.attacking = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hp.idleFunction(true);
        }
        hp.attacking = false;
    }
}
