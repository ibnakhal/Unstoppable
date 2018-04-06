using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Damage Stats")]
    public int damage;
    public bool hit;
    public bool left;
    public AudioSource source;
    public int upMod;

    [SerializeField]
    private List<string> targets;

    public void OnTriggerEnter2D (Collider2D other)
    {


        for (int x = 0; x < targets.Count; x++)
        {

            if (other.tag == targets[x] && hit)
            {
                Vector2 dir = new Vector2();

                if (!left)
                {
                    dir = Vector2.left;
                }
                else
                {
                    dir = Vector2.right;
                }
                other.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector2.up * upMod) + (dir * 1000));
                //other.GetComponent<Health>().Damaged(damage);
                Debug.Log(other.gameObject+ " Was hit");
                other.SendMessage("Damage", damage);
                source.Play();
            }
        }

    }




}
