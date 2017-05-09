using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAI : MonoBehaviour {
    Target target;
    private float aiAttackRate = 2f;
    private float minDistance = 5;
    private float lastAttackTime;
    NetworkEntity networkEntity;
    NetworkCommunication networkCommunication;

    void Start()
    {
        target = GetComponent<Target>();
        networkCommunication = GetComponent<NetworkCommunication>();
    }
    // Update is called once per frame
    void Update () {
        if (isReadyToAttackMinion())
        {
            FindClosestEnemy();
            lastAttackTime = Time.time;

        }
    }

    private bool isReadyToAttackMinion()
    {
        return (Time.time - lastAttackTime > aiAttackRate && GetComponent<CreepAi>() && GetComponent<CreepAi>().posibleTarget && GetComponent<CreepPlayer>());
    }
    private void FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyMinions");

        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float curDistance = GetDistanceBetweenPositions(enemy.transform.position, position);
            if (curDistance < minDistance)
            {
                if (GetComponent<CreepAi>().isAttacking == false)
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
