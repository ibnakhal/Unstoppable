using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Damage Stats")]
    public int damage;
    public bool hit;

    public void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Enemy" && hit)
        {
            other.GetComponent<Health>().Damaged(damage);
        }
    }




}
