using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAtack : MonoBehaviour { 

    private Transform bulletSpawnPoint;
    private GameObject target;
    private float minDistance = 3f;
    void Update()
    {
        TargetEnemy();
    }        
    void TargetEnemy()
    {

    }
    private void FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyMinions");
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {            
            float curDistance = GetDistanceBetweenPositions(enemy.transform.position, GetComponent<CreepAi>().posibleTarget.transform.position);
            if (curDistance < minDistance)
            {
                var attackMinionId = GetComponent<NetworkEntity>().Id;
                var networkEntityIdOfTarget = enemy.GetComponent<NetworkEntity>().Id;
                //networkCommunication.SendMinionDataToAttack(attackMinionId, networkEntityIdOfTarget);
                break;
            }            
        }

    }

    private float GetDistanceBetweenPositions(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow(end.y - start.y, 2));
    }
}

