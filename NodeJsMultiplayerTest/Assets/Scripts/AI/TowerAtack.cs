using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAtack : MonoBehaviour {

    private float minDistance = 3f;
    public GameObject trackingObject;
    float trackingRange = 10f;
    float fieldOfView = 360;
    public float lastTimeAttack;
    void Update()
    {
        if(isReadyToShot())
             TargetEnemy();
    } 
    private bool isReadyToShot()
    {
        return Time.time - lastTimeAttack > 2f;
    }       
    void TargetEnemy()
    {
        trackingObject = null;
        float hirange = float.MaxValue;
        Collider[] cols = Physics.OverlapSphere(transform.position, trackingRange);
        foreach (Collider col in cols)
        {
            GameObject target = col.gameObject;
            if (target != gameObject && target.CompareTag("EnemyMinions"))
                    {
                Vector3 dir = target.transform.position - transform.position;
                float range = dir.magnitude;
                float angle = Vector3.Angle(dir, transform.forward);
                if (range <= hirange && angle <= fieldOfView)
                {
                    hirange = range;
                    trackingObject = target;
                }
            }
        }
        if (trackingObject)
        {
            GetComponent<NetworkCommunication>().SendMinionsOrPlayerIdToServerForTowerAttacking(GetComponent<NetworkEntity>().Id, trackingObject.GetComponent<NetworkEntity>().Id);
        }


    }
   


}


