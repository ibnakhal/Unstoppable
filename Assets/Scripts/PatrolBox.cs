using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBox : MonoBehaviour
{
    public GameObject toDirect;
    public Vector2 dir;
    public float pauseTime;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == toDirect)
        {
            StartCoroutine(idePause());
        }
    }



    public IEnumerator idePause()
    {
        toDirect.GetComponent<Health>().idleFunction(true);

        yield return new WaitForSeconds(pauseTime);
        toDirect.transform.Rotate(new Vector3(0, 180, 0));
        toDirect.GetComponent<Health>().idleFunction(false);

    }
}
