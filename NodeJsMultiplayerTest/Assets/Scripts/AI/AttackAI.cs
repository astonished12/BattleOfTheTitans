using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAI : MonoBehaviour {

    private float aiAttackRate = 2f;
    private float minDistance = 3;
    private float lastAttackTime;
    private float lastTimeCheckPath;
    private float pathCheck = 0.2f;
    NetworkEntity networkEntity;
    NetworkCommunication networkCommunication;

    public bool noEnemyAround = false;
    void Start()
    {
        networkCommunication = GetComponent<NetworkCommunication>();
    }
    // Update is called once per frame
    void Update () {
        if (isReadyToAttackMinion())
        {
            FindClosestEnemy();
            lastAttackTime = Time.time;
        }
        if (isReadyToCheck())
        {
            ContinueMoving();
            lastTimeCheckPath = Time.time;
        }
    }

    private void ContinueMoving()
    {
         if(GetComponent<CreepAi>() && (GetComponent<CreepAi>().posibleTarget == null) || (GetDistanceBetweenPositions(transform.position,GetComponent<CreepAi>().posibleTarget.transform.position)> minDistance))
        {
            GetComponent<CreepAi>().posibleTarget = null;
            GetComponent<FollowerMinion>().mustStop = false;
            GetComponent<NetworkCommunication>().SendMinionHasNoEnemyAround(GetComponent<NetworkEntity>().Id);

        }
    }
    private bool isReadyToAttackMinion()
    {
        return (Time.time - lastAttackTime > aiAttackRate && GetComponent<CreepAi>() && GetComponent<CreepAi>().posibleTarget && GetComponent<CreepPlayer>());
    }

    private bool isReadyToCheck()
    {
        return (Time.time - lastTimeCheckPath > pathCheck);
    }
    private void FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyMinions");

        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (GetComponent<CreepAi>().posibleTarget)
            {
                float curDistance = GetDistanceBetweenPositions(enemy.transform.position, GetComponent<CreepAi>().posibleTarget.transform.position);
                if (curDistance < minDistance)
                {
                    var attackMinionId = GetComponent<NetworkEntity>().Id;
                    var networkEntityIdOfTarget = enemy.GetComponent<NetworkEntity>().Id;
                    networkCommunication.SendMinionDataToAttack(attackMinionId, networkEntityIdOfTarget);
                    break;
                }
            }
        } 

    }
    private float GetDistanceBetweenPositions(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow(end.y - start.y, 2));
    }
}
