using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damagePlayer = 10;
    public int damageMinion = 2;
    public int damageTower = 20;


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
            if (targetTransform != null && Vector3.Distance(transform.position, targetTransform.position) < 0.5f && ownerBullet!=null)
            {
                if (!ownerBullet.GetComponent<CreepAi>() && gameObject)
                {
                    targetTransform.GetComponent<Alive>().OnHit(ownerBullet, damagePlayer);
                }
                else if (ownerBullet.GetComponent<CreepAi>() && gameObject)
                {
                    targetTransform.GetComponent<Alive>().OnHit(ownerBullet, damageMinion);
                }
                else if (ownerBullet.GetComponent<NetworkEntity>().isTower)
                {
                    targetTransform.GetComponent<Alive>().OnHit(ownerBullet, damageTower);
                }

                DestroyImmediate(gameObject);
            }
        }
    }

}
