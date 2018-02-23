using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwavemovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector2(1, 0)*Time.deltaTime);
	}
}
