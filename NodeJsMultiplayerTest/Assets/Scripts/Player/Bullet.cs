using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage = 10;
    public float vel, acel, force;
    public Transform targetTransform;

    void Update()
    {
        if (targetTransform == null)
        {
            return;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, vel * Time.deltaTime);
            vel += acel * Time.deltaTime;
            acel = force * Time.deltaTime;
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.5f)
            {
                targetTransform.GetComponent<Alive>().OnHit(damage);
                Destroy(gameObject);
            }
        }
    }

}
