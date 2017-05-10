using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damagePlayer = 10;
    public int damageMinion = 2;

    public float vel, acel, force;
    public Transform targetTransform;
    public GameObject ownerBullet;
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
                if (!ownerBullet.GetComponent<CreepAi>())
                {
                    targetTransform.GetComponent<Alive>().OnHit(ownerBullet, damagePlayer);
                }
                else if (ownerBullet.GetComponent<CreepAi>())
                {
                    targetTransform.GetComponent<Alive>().OnHit(ownerBullet, damageMinion);
                }
                Destroy(gameObject);
            }
        }
    }

}
