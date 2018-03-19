using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangsterAIState : MonoBehaviour {

    [Header("Animations")]
    public Animator anim;


    public enum State
    {
       walking,
       idle,
       attacking,
       END,
    }

    public State status;

	
    
    // Use this for initialization
	void Start () {
        status = State.idle;
	}
	


	// Update is called once per frame
	void Update ()
    {
	switch(status)
        {
            case State.walking:
                anim.SetBool("walking", true);
                anim.SetBool("attacking", false);
                anim.SetBool("isIdle", false);
                break;
            case State.idle:
                anim.SetBool("isIdle", true);
                anim.SetBool("attacking", false);
                anim.SetBool("walking", false);
                break;
            case State.attacking:
                anim.SetBool("attacking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("walking", false);
                break;
        }
        

        if(status == State.walking)
        {
            this.gameObject.transform.Translate(Vector2.left * Time.deltaTime);
        }
        	
	}
}
