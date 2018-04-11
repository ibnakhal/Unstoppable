using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerAI : MonoBehaviour {

    public Transform gun;
    public GameObject bullet;
    public Animator anim;
    public float bulletSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hacker-01-Shoot"))
        {
            GameObject shot = Instantiate(bullet, gun.position, this.transform.rotation) as GameObject;
            if (this.GetComponent<SpriteRenderer>().flipX == true)
            {
                shot.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
            }
            else
            {
                shot.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
            }
        }
	}
}
