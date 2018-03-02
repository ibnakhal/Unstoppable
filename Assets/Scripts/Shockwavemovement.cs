using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwavemovement : MonoBehaviour {
    public float speed;
    public bool isleft;
	// Use this for initialization
	void Start () {
        if (isleft)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
